import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { FileParameter, ProductServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

class CreateProductDto {
  name: string;
  price: number;
  stockAmount: number | null;
  isActive: boolean;
  picture: FileParameter;
}

@Component({
  selector: 'app-create-product-dialog',
  templateUrl: './create-product-dialog.component.html'
})
export class CreateProductDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  product = new CreateProductDto();
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
    this.product.isActive = true;
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

    this._productService.create(
      this.product.name,
      this.product.price,
      this.product.stockAmount,
      this.product.isActive,
      this.product.picture
    ).subscribe(
      () => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      },
      () => {
        this.saving = false;
      }
    );
  }
}
