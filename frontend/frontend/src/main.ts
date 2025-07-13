import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';


// הוספת provideAnimations לפה:
bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),
    provideAnimations() // ⬅️ זו השורה החשובה
  ]
});

// bootstrapApplication(AppComponent, {
//   providers: [provideHttpClient()]
// });
