import { Component, computed, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  testName = signal('John Doe');
  testAge = signal(12);
  clipboardInfo = computed(() => {
    return `Name: ${this.testName()}, Age: ${this.testAge()}`;
  });

  changeNameOnClipboard() {
    this.testName.set('Jane Doe');
  }

  changeAgeOnClipboard() {
    this.testAge.set(100);
  }
}
