import { Component } from '@angular/core';
import { Article } from '../../../models/article.model';
import { CommonModule} from '@angular/common';
import { ShortenTextPipe } from '../../../pipes/shorten-text.pipe';
import { ArticleEndpointsService } from '../../../services/article-enpoints.service';
import { Router } from '@angular/router';

@Component({
  standalone:true,
  selector: 'app-article-list',
  imports: [CommonModule, ShortenTextPipe],
  templateUrl: './article-list.component.html',
  styleUrl: './article-list.component.css'
})
export class ArticleListComponent {
  articleList:Article[] = [];

  constructor(private articleService:ArticleEndpointsService, private router:Router) {
    // Generate dummy data on the meantime
    this.refreshList();
  }

  refreshList(){
    this.articleService.getArticles().subscribe({
      next: data => {
        this.articleList = data;
      },
      error: err =>{
        console.log(err);
      }
    });
  }

  goToArticle(id:Number){
    this.router.navigate(['view-article/'+id]);
  }
}
