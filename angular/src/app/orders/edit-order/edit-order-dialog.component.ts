import { Component, ElementRef, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AppComponentBase } from '@shared/app-component-base';
import { OrderDto, OrderedProductDto, OrderServiceProxy, ProductDto, ProductDtoPagedResultDto, ProductServiceProxy, UpdateOrderDto } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-edit-order-dialog',
  templateUrl: './edit-order-dialog.component.html',
})
export class EditOrderDialogComponent extends AppComponentBase implements OnInit {
  id: number;
  saving = false;
  order = new OrderDto();
  productNames: string[] = [];  // sync with order.orderedProducts
  activeProducts: ProductDto[];
  newOrderedProduct =  new OrderedProductDto();
  
  @Output() onSave = new EventEmitter<any>();
  @ViewChild('CreateOrderedProductForm') createForm: NgForm;
  @ViewChild('UpdateOrderTabset') tabset: TabsetComponent;
  @ViewChild('SelectInput') selectInput: ElementRef;

  constructor(
    injector: Injector,
    public _orderService: OrderServiceProxy,
    public _productService: ProductServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }
  
  ngOnInit(): void {
    this._orderService.get(this.id)
    .subscribe(result => {
      this.order = result;
      
      this.productNames = new Array(this.order.orderedProducts.length);
      for (let [index, ordered] of this.order.orderedProducts.entries()) {
        this.loadProductName(ordered.productId, index);
      }
    })

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

  protected loadProductName(id: number, index: number) {
    this._productService.get(id)
    .subscribe((result: ProductDto) => {
      this.productNames[index] = result.name;
    })
  }

  save(): void {
    this.saving = true;
    
    let newOrder: UpdateOrderDto = UpdateOrderDto.fromJS(this.order);    

    this._orderService.update(newOrder).subscribe(
      () => {
        this.notify.info(this.l('SuccessfullyUpdatedOrder'));
        this.bsModalRef.hide();
        this.onSave.emit();
      },
      () => {
        this.saving = false;
      }
    );
  }

  addOrderedProduct(): void {
    let selectOptions = this.selectInput.nativeElement.options
    let orderedName = selectOptions[selectOptions.selectedIndex].text;

    // validate amount
    this.newOrderedProduct.amount = Math.floor(this.newOrderedProduct.amount);
    if (this.newOrderedProduct.amount > this.activeProducts[selectOptions.selectedIndex].stockAmount) {
      abp.message.error(
        this.l('MinimunStockError')
      );
      return;
    }

    // validate duplicate product
    for (let ordered of this.order.orderedProducts) {
      if (ordered.productId == this.newOrderedProduct.productId) {
        abp.message.confirm(
          this.l('DuplicateProductError', orderedName, ordered.unitPrice, ordered.amount),
          this.l('AreYouSureWantToUpdateProduct', orderedName),
          (result: boolean) => {
            if (result) {
              Object.assign(
                this.order.orderedProducts.find(x => x.productId == this.newOrderedProduct.productId),
                this.newOrderedProduct
              )
              this.resetTabAndForm("SuccessfullyUpdatedProduct", orderedName);
            }
          }
        );
        return;
      }
    }

    this.order.orderedProducts.push(this.newOrderedProduct.clone());
    this.productNames.push(orderedName);
    this.resetTabAndForm("SuccessfullyAddedProduct", orderedName);
  }

  protected deleteFromOrder(product: OrderedProductDto): void {
    var productIndex = this.order.orderedProducts.findIndex(x => x.productId == product.productId);
    var productName = this.productNames[productIndex];
    
    abp.message.confirm(
      this.l('OrderedProductDeleteWarningMessage', productName),
      undefined,
      (result: boolean) => {
        if (result) {
          this.order.orderedProducts.splice(productIndex, 1);
          this.productNames.splice(productIndex, 1);
        }
      }
    );
  }

  protected getStockAmount(ordered: OrderedProductDto) {
    // address method being called multiple times for each click on ui
    let result = this.activeProducts.find(x => x.id == ordered.productId);
    return result ? result.stockAmount : 0;
  }

  protected autocompleteUnitPrice(): void {
    let selectedPrice = this.activeProducts[this.selectInput.nativeElement.options.selectedIndex].price;
    this.createForm.controls['unitPrice'].setValue(selectedPrice);
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
