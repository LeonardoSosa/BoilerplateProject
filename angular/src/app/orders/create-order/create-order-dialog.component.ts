import { Component, ElementRef, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AppComponentBase } from '@shared/app-component-base';
import { CreateOrderDto, CreateOrderedProductDto, OrderServiceProxy, ProductDto, ProductDtoPagedResultDto, ProductServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-create-order-dialog',
  templateUrl: './create-order-dialog.component.html'
})
export class CreateOrderDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  order = new CreateOrderDto();
  orderedNames: string[] = [];
  activeProducts: ProductDto[];       // possibleProducts, activeProducts

  orderedProduct = new CreateOrderedProductDto();

  @Output() onSave = new EventEmitter<any>();
  @ViewChild('createOrderedProductForm') createForm: NgForm;
  @ViewChild('CreateOrderTabset') tabset: TabsetComponent;
  @ViewChild('SelectInput') selectInput: ElementRef;
  @ViewChild('UnitPriceInput') unitPriceInput: ElementRef;

  constructor(
    injector: Injector,
    public _orderService: OrderServiceProxy,
    public _productService: ProductServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.order.orderedProducts = [];

    this._productService
    .getAll(
      undefined,
      true,
      0,
      999
    )
    .subscribe((result: ProductDtoPagedResultDto) => {
      this.activeProducts = result.items
    });
  }

  save(): void {
    this.saving = true;

    this._orderService.create(this.order).subscribe(
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

  addOrderedProduct(): void {
    let orderedName = this.selectInput.nativeElement.options[this.selectInput.nativeElement.options.selectedIndex].text;

    // amount has to be int
    this.orderedProduct.amount = Math.floor(this.orderedProduct.amount);

    // validate stock
    if (this.orderedProduct.amount > this.activeProducts[this.selectInput.nativeElement.options.selectedIndex].stockAmount) {
      abp.message.error(
        this.l('MinimunStockError')
      );
      return;
    }

    // validate duplicate product
    for (let ordered of this.order.orderedProducts) {
      if (ordered.productId == this.orderedProduct.productId) {
        abp.message.confirm(
          this.l('DuplicateProductError', orderedName, ordered.unitPrice, ordered.amount),
          this.l('AreYouSureWantToUpdateProduct', orderedName),
          (result: boolean) => {
            if (result) {
              Object.assign(
                this.order.orderedProducts.find(x => x.productId = this.orderedProduct.productId),
                this.orderedProduct
              )
              this.resetTabAndForm("SuccessfullyUpdatedProduct", orderedName);
            }
          }
        );
        return;
      }
    }

    this.order.orderedProducts.push(this.orderedProduct.clone());
    this.orderedNames.push(orderedName);
    this.resetTabAndForm("SuccessfullyAddedProduct", orderedName);
  }

  autocompleteUnitPrice(): void {
    this.createForm.controls['unitPrice'].setValue(this.activeProducts[this.selectInput.nativeElement.options.selectedIndex].price);
  }

  protected resetTabAndForm(message, interpolated?):void {
    this.tabset.tabs[0].active = true;
    abp.notify.info(this.l(message, interpolated));

    Object.keys(this.createForm.controls).forEach(key => {
      this.createForm.form.get(key).setValue(null);
    });
    this.createForm.form.markAsPristine();
    this.createForm.form.markAsUntouched();
    this.createForm.form.updateValueAndValidity();
  }
}
