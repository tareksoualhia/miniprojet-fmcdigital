import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { ClientService } from '../../services/client.service';
import { ProductService } from '../../services/product';
import { Order, CreateOrderDto } from '../../models/order';
import { Client } from '../../models/client';
import { Product } from '../../models/product';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './orders.html',
  styleUrl: './orders.scss'
})
export class Orders implements OnInit {
  orders: Order[] = [];
  clients: Client[] = [];
  products: Product[] = [];

  loading = true;
  errorMessage = '';
  successMessage = '';
  showForm = false;

  isEditMode = false;
  editingId: number | null = null;

  form: FormGroup;

  constructor(
    private orderService: OrderService,
    private clientService: ClientService,
    private productService: ProductService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      clientId: [null, Validators.required],
      lignes: this.fb.array([])
    });
  }

  ngOnInit(): void {
    this.loadOrders();
    this.clientService.getAll().subscribe({ next: (data: Client[]) => this.clients = data });
    this.productService.getAll().subscribe({ next: (data: Product[]) => this.products = data });
  }

  get lignes(): FormArray {
    return this.form.get('lignes') as FormArray;
  }

  loadOrders(): void {
    this.loading = true;
    this.orderService.getAll().subscribe({
      next: (data: Order[]) => {
        this.orders = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Erreur lors du chargement des commandes.';
        this.loading = false;
      }
    });
  }

  openCreateForm(): void {
    this.isEditMode = false;
    this.editingId = null;
    this.form.reset({ clientId: null });
    this.lignes.clear();
    this.addLigne();
    this.showForm = true;
    this.errorMessage = '';
  }

  openEditForm(order: Order): void {
    if (order.statut !== 'Brouillon') {
      this.errorMessage = 'Seules les commandes en brouillon peuvent être modifiées.';
      return;
    }

    this.isEditMode = true;
    this.editingId = order.id;
    this.errorMessage = '';

    this.lignes.clear();
    order.lignes.forEach(ligne => {
      this.lignes.push(this.fb.group({
        productId: [ligne.productId, Validators.required],
        quantite: [ligne.quantite, [Validators.required, Validators.min(1)]]
      }));
    });

    this.form.patchValue({ clientId: order.clientId });
    this.showForm = true;
  }

  cancelForm(): void {
    this.showForm = false;
    this.errorMessage = '';
  }

  addLigne(): void {
    this.lignes.push(this.fb.group({
      productId: [null, Validators.required],
      quantite: [1, [Validators.required, Validators.min(1)]]
    }));
  }

  removeLigne(index: number): void {
    this.lignes.removeAt(index);
  }

  getProductPrice(productId: number): number {
    const product = this.products.find(p => p.id === productId);
    return product ? product.prixUnitaireHT : 0;
  }

  getLigneTotal(index: number): number {
    const ligne = this.lignes.at(index).value;
    if (!ligne.productId || !ligne.quantite) return 0;
    return this.getProductPrice(ligne.productId) * ligne.quantite;
  }

  getFormTotalHT(): number {
    return this.lignes.controls.reduce((sum, _, i) => sum + this.getLigneTotal(i), 0);
  }

  getFormTotalTTC(): number {
    return Math.round(this.getFormTotalHT() * 1.19 * 100) / 100;
  }

  onSubmit(): void {
    if (this.form.invalid || this.lignes.length === 0) {
      this.form.markAllAsTouched();
      this.errorMessage = 'Veuillez remplir tous les champs obligatoires.';
      return;
    }

    const dto: CreateOrderDto = this.form.value;

    if (this.isEditMode && this.editingId !== null) {
      this.orderService.update(this.editingId, dto).subscribe({
        next: () => {
          this.successMessage = 'Commande modifiée avec succès.';
          this.showForm = false;
          this.loadOrders();
        },
        error: (err: any) => {
          this.errorMessage = err?.error?.message || 'Erreur lors de la modification de la commande.';
        }
      });
    } else {
      this.orderService.create(dto).subscribe({
        next: () => {
          this.successMessage = 'Commande créée avec succès.';
          this.showForm = false;
          this.loadOrders();
        },
        error: (err: any) => {
          this.errorMessage = err?.error?.message || 'Erreur lors de la création de la commande.';
        }
      });
    }
  }

  deleteOrder(id: number): void {
    if (!confirm('Voulez-vous vraiment supprimer cette commande ?')) return;

    this.orderService.delete(id).subscribe({
      next: () => {
        this.successMessage = 'Commande supprimée avec succès.';
        this.loadOrders();
      },
      error: (err: any) => {
        this.errorMessage = err?.error?.message || 'Erreur lors de la suppression.';
      }
    });
  }
}