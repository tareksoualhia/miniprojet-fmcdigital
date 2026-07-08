import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ClientService } from '../../../../services/client.service';
import { Client } from '../../../../models/client';

@Component({
  selector: 'app-client-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './client-detail.html',
  styleUrl: './client-detail.scss'
})
export class ClientDetail implements OnInit {
  client: Client | null = null;
  loading = true;
  errorMessage = '';

  constructor(private route: ActivatedRoute, private clientService: ClientService) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (!idParam) {
      this.errorMessage = 'Identifiant client manquant.';
      this.loading = false;
      return;
    }

    this.clientService.getById(+idParam).subscribe({
      next: (data: Client) => {
        this.client = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Client introuvable.';
        this.loading = false;
      }
    });
  }
}