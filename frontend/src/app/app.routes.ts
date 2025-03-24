import { Routes } from '@angular/router';
import { HomePageComponent } from './components/home-page/home-page.component';
import { ArticleListComponent } from './components/home-page/article-list/article-list.component';
import { ViewArticleComponent } from './components/home-page/view-article/view-article.component';
import { CreateArticleComponent } from './components/home-page/create-article/create-article.component';
import { UserLoginRegisterComponent } from './components/user-login-register/user-login-register.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { AccountSettingsComponent } from './components/home-page/account-settings/account-settings.component';
import { NotFoundPageComponent } from './components/shared/not-found-page/not-found-page.component';

export const routes: Routes = [
    {
        path: '',
        component: HomePageComponent,
        children: [
          { path: '', component: ArticleListComponent },
          { path: 'view-article/:id', component: ViewArticleComponent },
          { path: 'create-article', component: CreateArticleComponent },
          { path: 'account-settings', component: AccountSettingsComponent }
        ],
    },
    { path:'login', component: UserLoginRegisterComponent },
    { path:'register', component: UserRegisterComponent },
    { path: '**', component: NotFoundPageComponent}
];
