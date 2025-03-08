import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ScoredLead } from '../models/lead';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LeadService {
  private apiUrl = `${environment.apiUrl}/Lead`;

  constructor(private http: HttpClient) { }

  getLeads(): Observable<ScoredLead[]> {
    return this.http.get<ScoredLead[]>(this.apiUrl);
  }

  generateLeads(searchQuery: string): Observable<ScoredLead[]> {
    return this.http.post<ScoredLead[]>(`${this.apiUrl}/generate`, { searchQuery });
  }

  getLead(id: number): Observable<ScoredLead> {
    return this.http.get<ScoredLead>(`${this.apiUrl}/${id}`);
  }
} 