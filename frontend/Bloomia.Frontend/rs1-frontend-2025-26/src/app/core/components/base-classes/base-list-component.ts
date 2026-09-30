// src/app/core/components/base-list.component.ts

import {BaseComponent} from './base-component';

export abstract class BaseListComponent<TItem> extends BaseComponent{
  items: TItem[] = [];

  /**
   * The actual data-loading implementation is left to the subclass.
   */
  protected abstract loadData(): void;

  /**
   * Helper you can call from a child component's ngOnInit.
   */
  protected initList(): void {
    this.loadData();
  }
}
