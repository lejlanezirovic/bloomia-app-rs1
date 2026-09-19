import { Component, inject, OnInit } from '@angular/core';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';
import { ListMyClientsQueryDto, ListMyClientsRequest } from '../../../api-services/therapist-my-clients/my-clients-api.model';
import { TherapistsApiService } from '../../../api-services/therapists/therapists-api.service';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-clients',
  standalone: false,
  templateUrl: './my-clients.component.html',
  styleUrl: './my-clients.component.scss',
})
export class MyClientsComponent extends BaseListPagedComponent<ListMyClientsQueryDto, ListMyClientsRequest> implements OnInit {

  private api = inject(TherapistsApiService);
  private fb = inject(FormBuilder);
  private router = inject(Router);


  form = this.fb.group({
    search: ['']
  });

  constructor() {
    super();

    this.request = new ListMyClientsRequest();
    this.request.paging.pageSize = 4;
  }

  ngOnInit(): void {
    this.initList();
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.api.listMyClients(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading('Failed to load clients');
        console.error('Load clients error:', err);
      }
    });
  }

  onSearch(): void {
    this.request.search = this.form.value.search ?? null;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  clearSearch(): void {
    this.form.reset({
      search: ''
    });

    this.request.search = null;
    this.request.paging.page = 1;
    
    this.loadPagedData();
  }

  openChat(clientId: number, clientFullName: string): void {
    this.router.navigate(['/therapist/direct-chats', clientId, 'direct-chats-details'],
      {
        queryParams: {
          clientFullName: clientFullName
        }
      }
    );
  }

}
