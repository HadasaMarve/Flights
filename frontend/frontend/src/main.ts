import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.config';
import { provideAnimations } from '@angular/platform-browser/animations';

platformBrowserDynamic([
  provideAnimations()
]).bootstrapModule(AppModule)
  .catch(err => console.error(err));
