import { Component, computed, signal } from "@angular/core";

@Component({
    // name to use as html tag in html markup
    selector: 'test-component',
    template: `
    <div>
        <h2>Clipboard Info: {{clipboardInfo()}}</h2>
        <button (click)="changeNameOnClipboard()">Tamper with Name</button>
        <button (click)="changeAgeOnClipboard()">Tamper with Age</button>
    </div>`,

      styles: `h1 { font-size: 3em; } `,
})
export class TestComponent {
    testName = signal("John Doe");
    testAge = signal(12);
    clipboardInfo = computed(() => {
        return `Name: ${this.testName()}, Age: ${this.testAge()}`;
    });

    changeNameOnClipboard() {
        this.testName.set("Jane Doe");
    };

    changeAgeOnClipboard() {
        this.testAge.set(100);
    }
}