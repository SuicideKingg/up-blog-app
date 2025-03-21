import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'shortenText'
})
export class ShortenTextPipe implements PipeTransform {

  transform(value:string,maxLength:number){
    const length = value.length;
    if(length <= maxLength){
      // just return the value
      return value;
    }
    else{
      const suffix = value && value.length > maxLength ? "..." : "";
      return value.slice(0,maxLength) + suffix;
    }
  }

}
