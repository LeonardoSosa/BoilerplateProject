import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { FileParameter, ProductServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

class UpdateProductDto {
  id: number;
  name: string;
  price: number;
  stockAmount: number;
  isActive: boolean;
  picture: FileParameter | null;
}

@Component({
  selector: 'app-edit-product-dialog',
  templateUrl: './edit-product-dialog.component.html',
})
export class EditProductDialogComponent extends AppComponentBase implements OnInit{
  id: number;
  saving = false;
  product = new UpdateProductDto();
  selectedFile: File;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _productService: ProductServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._productService.get(this.id)
    .subscribe(result => {
      Object.assign(this.product, result)
    })
  }

  onFileChange(event) {
    this.selectedFile = event.target.files ? event.target.files[0] : null;
  }


  save(): void {
    this.saving = true;

    if (this.selectedFile) {
      this.product.picture = {
        data: this.selectedFile,
        fileName: this.selectedFile.name
      };
    }

    this._productService.update(
      this.product.name,
      this.product.price,
      this.product.stockAmount,
      this.product.isActive,
      this.product.picture,
      this.product.id
    ).subscribe(
      () => {
        this.notify.info(this.l('SuccessfullyUpdatedProduct'));
        this.bsModalRef.hide();
        this.onSave.emit();
      },
      () => {
        this.saving = false;
      }
    );
  }
}
