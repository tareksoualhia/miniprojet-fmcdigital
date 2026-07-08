import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductService } from '../../../services/product';
import { Product } from '../../../models/product';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss'
})
export class ProductDetail implements OnInit {
  product: Product | null = null;
  loading = true;
  errorMessage = '';

  constructor(private route: ActivatedRoute, private productService: ProductService) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (!idParam) {
      this.errorMessage = 'Identifiant produit manquant.';
      this.loading = false;
      return;
    }

    this.productService.getById(+idParam).subscribe({
      next: (data: Product) => {
        this.product = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Produit introuvable.';
        this.loading = false;
      }
    });
  }
}