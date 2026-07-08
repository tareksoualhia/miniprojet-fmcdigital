
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../services/product';
import { Product, CreateProductDto } from '../../models/product';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterLink],
  templateUrl: './products.html',
  styleUrl: './products.scss'
})
export class Products implements OnInit {
  products: Product[] = [];
  loading = true;
  errorMessage = '';
  successMessage = '';

  showForm = false;
  isEditMode = false;
  editingId: number | null = null;
  form: FormGroup;

  constructor(private productService: ProductService, private fb: FormBuilder) {
    this.form = this.fb.group({
      reference: ['', Validators.required],
      nom: ['', Validators.required],
      description: [''],
      prixUnitaireHT: [0, [Validators.required, Validators.min(0)]],
      quantiteEnStock: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getAll().subscribe({
      next: (data: Product[]) => {
        this.products = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Erreur lors du chargement des produits.';
        this.loading = false;
      }
    });
  }

  openCreateForm(): void {
    this.isEditMode = false;
    this.editingId = null;
    this.form.reset({ reference: '', nom: '', description: '', prixUnitaireHT: 0, quantiteEnStock: 0 });
    this.showForm = true;
  }

  openEditForm(product: Product): void {
    this.isEditMode = true;
    this.editingId = product.id;
    this.form.patchValue(product);
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

    const dto: CreateProductDto = this.form.value;

    if (this.isEditMode && this.editingId !== null) {
      this.productService.update(this.editingId, dto).subscribe({
        next: () => {
          this.successMessage = 'Produit modifié avec succès.';
          this.showForm = false;
          this.loadProducts();
        },
        error: (err: any) => {
          this.errorMessage = err?.error?.message || 'Erreur lors de la modification.';
        }
      });
    } else {
      this.productService.create(dto).subscribe({
        next: () => {
          this.successMessage = 'Produit créé avec succès.';
          this.showForm = false;
          this.loadProducts();
        },
        error: (err: any) => {
          this.errorMessage = err?.error?.message || 'Erreur lors de la création.';
        }
      });
    }
  }

  deleteProduct(id: number): void {
    if (!confirm('Voulez-vous vraiment supprimer ce produit ?')) return;

    this.productService.delete(id).subscribe({
      next: () => {
        this.successMessage = 'Produit supprimé avec succès.';
        this.loadProducts();
      },
      error: (err: any) => {
        this.errorMessage = err?.error?.message || 'Erreur lors de la suppression.';
      }
    });
  }
}