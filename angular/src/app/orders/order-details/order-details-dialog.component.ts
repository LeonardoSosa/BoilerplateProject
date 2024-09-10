import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { OrderDto, OrderServiceProxy, ProductDto, ProductServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-order-details-dialog',
  templateUrl: './order-details-dialog.component.html',
})
export class OrderDetailsDialogComponent extends AppComponentBase implements OnInit{
  order: OrderDto;
  productNames: string[] = [];

  constructor(
    injector: Injector,
    public _orderService: OrderServiceProxy,
    public _productService: ProductServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    for (let ordered of this.order.orderedProducts) {
      this._productService
      .get(ordered.productId)
      .subscribe((result: ProductDto) => {
        this.productNames.push(result.name);
      })
    }
  }
}
