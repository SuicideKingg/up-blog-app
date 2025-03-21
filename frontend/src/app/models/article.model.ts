import { CommentArticle } from "./comment.model";

export class Article {
    id:number = 0;
    title:string = '';
    content:string = '';
    datePosted:Date=new Date();
    author:string='';
    authorId:number=0;
    comments:CommentArticle[] = [];
    AuthorAvatarURL:string='';
}
