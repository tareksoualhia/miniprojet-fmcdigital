export interface Product {
  id: number;
  reference: string;
  nom: string;
  description: string;
  prixUnitaireHT: number;
  quantiteEnStock: number;
  dateCreation: string;
}

export interface CreateProductDto {
  reference: string;
  nom: string;
  description: string;
  prixUnitaireHT: number;
  quantiteEnStock: number;
}

export interface UpdateProductDto extends CreateProductDto {}