import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { FileParameter, ProductServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

class CreateProductDto {
  name: string;
  price: number;
  stockAmount: number;
  isActive: boolean | null;
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
    // ERROR InvalidStateError: Failed to set the 'value' property on 'HTMLInputElement'...
    this.selectedFile = event.target.files ? event.target.files[0] : null;

    // ERROR TypeError: Cannot read properties of undefined (reading 'name')
    // ERROR TypeError: Failed to execute 'append' on 'FormData': parameter 2 is not of type 'Blob' -> quando tenta apertar o botao salvar
    // this.product.picture = {
    //   data: this.selectedFile,
    //   fileName: this.selectedFile.name
    // };

    // ERROR TypeError: Cannot create property 'data' on string 'C:\fakepath\banana.png'
    // this.product.picture.data = event.target.files[0]
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
      this.product.picture)
    .subscribe(
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
