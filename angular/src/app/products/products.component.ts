import { Component, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { ProductDto, ProductDtoPagedResultDto, ProductServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateProductDialogComponent } from './create-product/create-product-dialog.component';
import { finalize } from 'rxjs/operators';
import { EditProductDialogComponent } from './edit-product/edit-product-dialog.component';

class PagedProductsRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrl: './products.component.css',
  animations: [appModuleAnimation()],
})
export class ProductsComponent  extends PagedListingComponentBase<ProductDto>{
  products: ProductDto[] = [];
  picturesBase64: string[] = [];
  keywordFilter = '';
  isActiveFilter: boolean | null;
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _productService: ProductServiceProxy,
    private _modalService: BsModalService,
  ) {
    super(injector)
  }

  createProduct(): void {
    this.showCreateOrEditProductDialog();
  }

  editProduct(product: ProductDto): void {
    this.showCreateOrEditProductDialog(product.id);
  }

  protected list(
    request: PagedProductsRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keywordFilter;
    request.isActive = this.isActiveFilter;

    this._productService
      .getAll(
        request.keyword,
        request.isActive,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: ProductDtoPagedResultDto) => {
        this.products = result.items;
        
        this.picturesBase64 = new Array(this.products.length);
        for (let [index, product] of this.products.entries()) {
          this.loadPicture(product.id, index);
        }

        this.showPaging(result, pageNumber);
      });
  }

  protected loadPicture(id: number, index: number) {
    this._productService.getProductPicture(id)
    .subscribe({next: (response: any) => {
        this.picturesBase64[index] = (`data:image/png;base64,${response}`); // Prefix the base64 data with correct format
      }
    })
  }

  protected delete(product: ProductDto): void {
    abp.message.confirm(
      this.l('ProductDeleteWarningMessage', product.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._productService.delete(product.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }

  showCreateOrEditProductDialog(id?: number): void {
    let createOrEditProductDialog: BsModalRef;

    if (!id) {
      createOrEditProductDialog = this._modalService.show(
        CreateProductDialogComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditProductDialog = this._modalService.show(
        EditProductDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      ); 
    }

    createOrEditProductDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }

  clearFilters(): void {
    this.keywordFilter = '';
    this.isActiveFilter = undefined;
    this.getDataPage(1);
  }
}
