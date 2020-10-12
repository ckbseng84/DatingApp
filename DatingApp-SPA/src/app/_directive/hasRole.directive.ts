import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  isVisible = false;

  constructor(private viewContrainerRef: ViewContainerRef,
              private templateRef: TemplateRef<any>,
              private authSerivce: AuthService) { }
  ngOnInit(): void {
    const userRoles = this.authSerivce.decodedToken.role as Array<string>;

    // if no roles clear the viewContainerRef
    if (!userRoles){
      this.viewContrainerRef.clear();
    }

    // if user has role need then render the element
    if (this.authSerivce.roleMatch(this.appHasRole)){
      if (!this.isVisible){
        this.isVisible = true;
        this.viewContrainerRef.createEmbeddedView(this.templateRef);
      }else{
        this.isVisible = false;
        this.viewContrainerRef.clear();
      }
    }
  }

}
