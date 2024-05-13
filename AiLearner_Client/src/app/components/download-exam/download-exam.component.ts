import { Component } from '@angular/core';
import { PdfService } from '../../services/pdf.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-download-exam',
  standalone: true,
  imports: [],
  templateUrl: './download-exam.component.html',
  styleUrl: './download-exam.component.css'
})
export class DownloadExamComponent {
  constructor(private pdfService: PdfService,private route: ActivatedRoute ) {}
  
  downloadPdf() {
    const materialId = this.route.snapshot.paramMap.get('id');
    this.pdfService.getPDF(parseInt(materialId!)).subscribe((response) => {
      let fileName = response.headers.get('content-disposition')?.split(';')[1].split('=')[1].slice(0, -1).slice(1);
      console.log(response.headers.get('content-disposition'));
      console.log(response.headers.get('content-disposition')?.split(';')[1].split('=')[1]);
      
      let blob: Blob = response.body as Blob;
      let a = document.createElement('a');
      console.log(fileName);
      a.download = fileName!;
      a.href = window.URL.createObjectURL(blob);
      a.click();
    });
  }
}
