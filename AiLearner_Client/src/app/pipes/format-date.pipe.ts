import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatDate',
  standalone: true,
})
export class FormatDatePipe implements PipeTransform {
  transform(value: string | Date): string {
    if (!value) {
      return '';
    }

    // Convert Date to string if the input is a Date object
    let dateString: string;
    if (value instanceof Date) {
      // Converting to a string in ISO format (YYYY-MM-DDTHH:mm:ss.sssZ)
      dateString = value.toISOString();
    } else {
      dateString = value;
    }

    // Extracting date and time parts
    const parts = dateString.split('T');
    const datePart = parts[0];
    //const timePart = parts[1].substring(0, 5); // Getting HH:mm

    // Reformatting the date to YYYY/MM/DD
    const formattedDate = datePart.replace(/-/g, '/');

    return formattedDate;
  }
}
