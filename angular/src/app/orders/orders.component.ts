import { Component, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { OrderDto, OrderDtoPagedResultDto, OrderServiceProxy, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrderDialogComponent } from './create-order/create-order-dialog.component';
import { OrderDetailsDialogComponent } from './order-details/order-details-dialog.component';
import { finalize } from 'rxjs/operators';

class PagedOrdersRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  animations: [appModuleAnimation()]
})
export class OrdersComponent extends PagedListingComponentBase<OrderDto>{
  orders: OrderDto[] = [];
  keyword = '';
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _orderService: OrderServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector)
  }

  createOrder(): void {
    this.ShowCreateOrderDialog();
  }

  protected list(
    request: PagedOrdersRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._orderService
      .getAll(
        request.keyword,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: OrderDtoPagedResultDto) => {
        this.orders = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  protected delete(order: OrderDto): void {
    abp.message.confirm(
      this.l('OrderDeleteWarningMessage', order.id),
      undefined,
      (result: boolean) => {
        if (result) {
          this._orderService.delete(order.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }

  getOrderDetails(order: OrderDto): void {
    this.ShowOrderDetailsDialog(order);
  }

  ShowCreateOrderDialog(): void {
    let createOrderDialog: BsModalRef;
    createOrderDialog = this._modalService.show(CreateOrderDialogComponent, {
      class: "modal-lg",
    });

    createOrderDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }

  ShowOrderDetailsDialog(order: OrderDto): void {
    let OrderDetailsDialog: BsModalRef;
    OrderDetailsDialog = this._modalService.show(OrderDetailsDialogComponent, {
      class: "modal-lg",
      initialState: {
        order: order,
      }
    });
  }

  clearFilters(): void {
    this.keyword = '';
    this.getDataPage(1);
  }
}
