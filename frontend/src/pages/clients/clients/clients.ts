import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ClientService } from '../../../services/client.service';
import { Client, CreateClientDto } from '../../../models/client';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-clients',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterLink],
  templateUrl: './clients.html',
  styleUrl: './clients.scss'
})
export class ClientsComponent implements OnInit {
  clients: Client[] = [];
  loading = true;
  errorMessage = '';
  successMessage = '';

  showForm = false;
  isEditMode = false;
  editingId: number | null = null;
  form: FormGroup;

  constructor(private clientService: ClientService, private fb: FormBuilder) {
    this.form = this.fb.group({
      nom: ['', Validators.required],
      prenomOuRaisonSociale: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      telephone: ['', Validators.required],
      adresse: ['']
    });
  }

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients(): void {
    this.loading = true;
    this.clientService.getAll().subscribe({
      next: (data: Client[]) => {
        this.clients = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Erreur lors du chargement des clients.';
        this.loading = false;
      }
    });
  }

  openCreateForm(): void {
    this.isEditMode = false;
    this.editingId = null;
    this.form.reset({ nom: '', prenomOuRaisonSociale: '', email: '', telephone: '', adresse: '' });
    this.showForm = true;
  }

  openEditForm(client: Client): void {
    this.isEditMode = true;
    this.editingId = client.id;
    this.form.patchValue(client);
    this.showForm = true;
  }

  cancelForm(): void {
    this.showForm = false;
    this.errorMessage = '';
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const dto: CreateClientDto = this.form.value;

    if (this.isEditMode && this.editingId !== null) {
      this.clientService.update(this.editingId, dto).subscribe({
        next: () => {
          this.successMessage = 'Client modifié avec succès.';
          this.showForm = false;
          this.loadClients();
        },
        error: (err: any) => {
          this.errorMessage = err?.error?.message || 'Erreur lors de la modification.';
        }
      });
    } else {
      this.clientService.create(dto).subscribe({
        next: () => {
          this.successMessage = 'Client créé avec succès.';
          this.showForm = false;
          this.loadClients();
        },
        error: (err: any) => {
          this.errorMessage = err?.error?.message || 'Erreur lors de la création.';
        }
      });
    }
  }

  deleteClient(id: number): void {
    if (!confirm('Voulez-vous vraiment supprimer ce client ?')) return;

    this.clientService.delete(id).subscribe({
      next: () => {
        this.successMessage = 'Client supprimé avec succès.';
        this.loadClients();
      },
      error: (err: any) => {
        this.errorMessage = err?.error?.message || 'Erreur lors de la suppression.';
      }
    });
  }
}