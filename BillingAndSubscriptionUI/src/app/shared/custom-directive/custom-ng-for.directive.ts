import {
  Directive,
  Input,
  OnChanges,
  SimpleChanges,
  TemplateRef,
  ViewContainerRef
} from '@angular/core';

@Directive({
  selector: '[appCustomNgFor]',
  standalone: false
})
export class CustomNgForDirective<T> implements OnChanges {
  @Input() appCustomForOf: T[] = [];
  @Input() appCustomForFilter: (item: T) => boolean = () => true;
  @Input() appCustomForSort: (a: T, b: T) => number = () => 0;

  constructor(
    private readonly viewContainer: ViewContainerRef,
    private readonly template: TemplateRef<any>
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['appCustomForOf']) {
      this.updateView();
    }
  }

  private updateView(): void {
    this.viewContainer.clear();
    const filteredAndSortedItems = this.appCustomForOf
      .filter(this.appCustomForFilter)
      .sort(this.appCustomForSort);

    filteredAndSortedItems.forEach((item, index) => {
      this.viewContainer.createEmbeddedView(this.template, {
        $implicit: item,
        index: index
      });
    });
  }
}
