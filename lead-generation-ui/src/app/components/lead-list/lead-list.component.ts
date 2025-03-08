import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { LeadService } from '../../services/lead.service';
import { ScoredLead } from '../../models/lead';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-lead-list',
  templateUrl: './lead-list.component.html',
  styleUrls: ['./lead-list.component.scss']
})
export class LeadListComponent implements OnInit {
  leads: ScoredLead[] = [];
  loading = false;
  error = '';
  searchControl = new FormControl('');
  generating = false;

  constructor(private leadService: LeadService) { }

  ngOnInit() {
    this.loadLeads();

    // Setup search with debounce
    this.searchControl.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged()
      )
      .subscribe(query => {
        if (query) {
          this.generateLeads(query);
        }
      });
  }

  loadLeads() {
    this.loading = true;
    this.leadService.getLeads()
      .subscribe({
        next: (leads) => {
          this.leads = leads;
          this.loading = false;
        },
        error: (error) => {
          this.error = error.message;
          this.loading = false;
        }
      });
  }

  generateLeads(query: string) {
    this.generating = true;
    this.leadService.generateLeads(query)
      .subscribe({
        next: (leads) => {
          this.leads = leads;
          this.generating = false;
        },
        error: (error) => {
          this.error = error.message;
          this.generating = false;
        }
      });
  }
} 