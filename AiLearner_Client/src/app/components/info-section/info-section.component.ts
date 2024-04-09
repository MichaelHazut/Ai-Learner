import {
  Component,
  ViewChildren,
  QueryList,
  ElementRef,
  AfterViewInit,
} from '@angular/core';
import {
  trigger,
  transition,
  style,
  animate,
  state,
} from '@angular/animations';

@Component({
  selector: 'app-info-section',
  standalone: true,
  templateUrl: './info-section.component.html',
  styleUrls: ['./info-section.component.css'], // Corrected from styleUrl to styleUrls
  animations: [
    // trigger('visibilityChanged', [
    //   state('inViewport', style({ opacity: 1, transform: 'translateX(0)' })),
    //   state('outOfViewport', style({ opacity: 0, transform: 'translateX(60%)' })),
    //   transition('outOfViewport => inViewport', animate('2s ease-in')),
    //   transition('inViewport => outOfViewport', animate('2s ease-out')),
    // ]),
  ],
})
export class InfoSectionComponent implements AfterViewInit {
  @ViewChildren('card1, card2, card3, card4', { read: ElementRef }) 
  cards!: QueryList<ElementRef>;

  ngAfterViewInit() {
    this.cards.forEach((card) => {
      const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            console.log("entry.target: ", entry.target, " added class animate-right-load");
            entry.target.classList.add('animate-right-load');
          } else {
            entry.target.classList.remove('animate-right-load');
            console.log("entry.target: ", entry.target, " removed class animate-right-load");
          }
        });
      }, { threshold: 0.1 });

      observer.observe(card.nativeElement);
    });
  }

}
