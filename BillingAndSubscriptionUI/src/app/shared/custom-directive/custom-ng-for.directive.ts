import { Directive, Input, OnChanges, SimpleChanges, TemplateRef, ViewContainerRef } from '@angular/core';

interface CustomNgForContext<T> {
  $implicit: T;
  index: number;
}

@Directive({
  selector: '[appCustomNgFor][appCustomNgForOf]',
  standalone: false
})
export class CustomNgForDirective<T> implements OnChanges {
  @Input() appCustomNgForOf: T[] = [];

  constructor(
    private readonly viewContainer: ViewContainerRef,
    private readonly template: TemplateRef<CustomNgForContext<T>>
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['appCustomNgForOf']) {
      this.updateView();
    }
  }

  private updateView(): void {
    this.viewContainer.clear();

    this.appCustomNgForOf.forEach((item, index) => {
      this.viewContainer.createEmbeddedView(this.template, {
        $implicit: item,
        index
      });
    });
  }
}
