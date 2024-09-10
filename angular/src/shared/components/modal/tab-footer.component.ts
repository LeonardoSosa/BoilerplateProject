import {
  Component,
  Input,
  ChangeDetectionStrategy,
  Injector,
  Output,
  EventEmitter,
} from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'tab-footer',
  templateUrl: './tab-footer.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TabFooterComponent extends AppComponentBase {
  @Input() addLabel = this.l('Add');
  @Input() addDisabled: boolean;

  @Output() customOnClick = new EventEmitter<any>();

  constructor(injector: Injector) {
    super(injector);
  }
}
