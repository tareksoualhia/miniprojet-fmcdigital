import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { Order } from '../../../models/order';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.scss'
})
export class OrderDetail implements OnInit {
  order: Order | null = null;
  loading = true;
  errorMessage = '';
  successMessage = '';
  orderId!: number;

  constructor(private route: ActivatedRoute, private orderService: OrderService) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (!idParam) {
      this.errorMessage = 'Identifiant de commande manquant.';
      this.loading = false;
      return;
    }
    this.orderId = +idParam;
    this.loadOrder();
  }

  loadOrder(): void {
    this.loading = true;
    this.orderService.getById(this.orderId).subscribe({
      next: (data: Order) => {
        this.order = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Commande introuvable.';
        this.loading = false;
      }
    });
  }

  validateOrder(): void {
    if (!confirm('Confirmer la validation de cette commande ? Le stock des produits sera mis à jour.')) return;

    this.orderService.validate(this.orderId).subscribe({
      next: () => {
        this.successMessage = 'Commande validée avec succès. Le stock a été mis à jour.';
        this.loadOrder();
      },
      error: (err: any) => {
        this.errorMessage = err?.error?.message || 'Erreur lors de la validation.';
      }
    });
  }
}