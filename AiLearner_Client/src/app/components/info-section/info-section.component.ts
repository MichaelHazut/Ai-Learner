import {
  Component,
  ViewChildren,
  QueryList,
  ElementRef,
  AfterViewInit,
} from '@angular/core';


@Component({
  selector: 'app-info-section',
  standalone: true,
  templateUrl: './info-section.component.html',
  styleUrls: ['./info-section.component.css'], 
})
export class InfoSectionComponent implements AfterViewInit {
  @ViewChildren('card1, card2, card3, card4', { read: ElementRef })
  cards!: QueryList<ElementRef>;

  ngAfterViewInit() {
    this.cards.forEach((card) => {
      const observer = new IntersectionObserver(
        (entries) => {
          entries.forEach((entry) => {
            if (entry.isIntersecting) {
              entry.target.classList.add('animate-right-load');
            } else {
              entry.target.classList.remove('animate-right-load');
            }
          });
        },
        { threshold: 0.1 }
      );

      observer.observe(card.nativeElement);
    });
  }
}
