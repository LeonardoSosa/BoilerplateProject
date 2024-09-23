import { Component, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { OrderDto, OrderDtoPagedResultDto, OrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrderDialogComponent } from './create-order/create-order-dialog.component';
import { OrderDetailsDialogComponent } from './order-details/order-details-dialog.component';
import { finalize } from 'rxjs/operators';
import { EditOrderDialogComponent } from './edit-order/edit-order-dialog.component';

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
  keywordFilter = '';
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _orderService: OrderServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector)
  }

  createOrder(): void {
    this.showCreateOrEditOrderDialog();
  }

  editOrder(order: OrderDto): void {
    this.showCreateOrEditOrderDialog(order.id);
  }

  protected list(
    request: PagedOrdersRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keywordFilter;

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
    this.showOrderDetailsDialog(order);
  }

  showCreateOrEditOrderDialog(id?: number): void {
    let createOrEditOrderDialog: BsModalRef;

    if (!id) {
      createOrEditOrderDialog = this._modalService.show(
        CreateOrderDialogComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditOrderDialog = this._modalService.show(
        EditOrderDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      ); 
    }

    createOrEditOrderDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }

  showOrderDetailsDialog(order: OrderDto): void {
    let OrderDetailsDialog: BsModalRef;
    OrderDetailsDialog = this._modalService.show(OrderDetailsDialogComponent, {
      class: "modal-lg",
      initialState: {
        order: order,
      }
    });
  }

  clearFilters(): void {
    this.keywordFilter = '';
    this.getDataPage(1);
  }
}
