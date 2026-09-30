import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { SelfTestsApiService } from '../../../api-services/selfTests/selfTests-api.service';
import { ListMySelfTestResultDto, ListMySelfTestResultsQuery } from '../../../api-services/selfTests/selfTests-api.models';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';
import { ToasterService } from '../../../core/services/toaster.service';
import { FitConfirmDialogComponent } from '../../shared/components/fit-confirm-dialog/fit-confirm-dialog.component';
import { DialogType, DialogButton, DialogResult } from '../../shared/models/dialog-config.model';

@Component({
  selector: 'app-my-self-tests',
  standalone: false,
  templateUrl: './my-self-tests.component.html',
  styleUrl: './my-self-tests.component.scss',
})
export class MySelfTestsComponent extends BaseListPagedComponent<ListMySelfTestResultDto, ListMySelfTestResultsQuery> implements OnInit {

  private apiService = inject(SelfTestsApiService);
  private toasterService = inject(ToasterService);
  private dialog = inject(MatDialog);
  private fb = inject(FormBuilder);

  selfTestResults: ListMySelfTestResultDto[] = [];

  filterForm: FormGroup = this.fb.group({
    testName: ['', [Validators.maxLength(100)]],
    completedFrom: [null],
    completedTo: [null],
    minAverage: [null, [Validators.min(1), Validators.max(5)]],
    maxAverage: [null, [Validators.min(1), Validators.max(5)]],
  });

  editingResultId: number | null = null;
  editNoteControl = this.fb.control('', [Validators.required, Validators.maxLength(500)]);

  constructor() {
    super();
    this.request = new ListMySelfTestResultsQuery();
  }

  ngOnInit(): void {
    this.initList();
  }

  loadPagedData(): void {
    this.startLoading();

    this.apiService.getMySelfTestResults(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.selfTestResults = response.items;
        this.stopLoading();
      },
      error: (err) => {
        this.errorMessage = 'Failed to load self test results';
        this.toasterService.error('Failed to load your self test results.');
        this.stopLoading();
      }
    });
  }

  applyFilters(): void {
    if (this.filterForm.invalid) {
      this.filterForm.markAllAsTouched();
      return;
    }

    const value = this.filterForm.value;
    this.request.testName = value.testName?.trim() || null;
    this.request.completedFrom = value.completedFrom || null;
    this.request.completedTo = value.completedTo || null;
    this.request.minAverage = value.minAverage ?? null;
    this.request.maxAverage = value.maxAverage ?? null;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  toggleSortByDate(): void {
    this.request.sortByDateDesc = !this.request.sortByDateDesc;
    this.loadPagedData();
  }

  startEditingNote(result: ListMySelfTestResultDto): void {
    this.editingResultId = result.resultId;
    this.editNoteControl.setValue(result.clientNote ?? '');
  }

  cancelEditingNote(): void {
    this.editingResultId = null;
  }

  saveNote(result: ListMySelfTestResultDto): void {
    if (this.editNoteControl.invalid) {
      this.editNoteControl.markAsTouched();
      return;
    }

    const newNote = this.editNoteControl.value ?? '';
    this.apiService.updateMySelfTestResultNote(result.resultId, { clientNote: newNote }).subscribe({
      next: (response) => {
        result.clientNote = response.clientNote;
        this.editingResultId = null;
        this.toasterService.success('Note updated successfully.');
      },
      error: (err) => {
        this.toasterService.error('Failed to update the note. Try again.');
        console.error(err);
      }
    });
  }

  openConfirmDialogToDelete(result: ListMySelfTestResultDto): void {
    const dialogRef = this.dialog.open(FitConfirmDialogComponent, {
      width: '400px',
      data: {
        type: DialogType.WARNING,
        title: 'Confirm removing self test result',
        message: `Are you sure you want to remove the result for "${result.selfTestName}"?`,
        buttons: [
          { type: DialogButton.CANCEL, color: 'primary' },
          { type: DialogButton.DELETE, color: 'warn', label: 'Yes, remove' }
        ]
      }
    });

    dialogRef.afterClosed().subscribe((dialogResult: DialogResult | undefined) => {
      if (dialogResult?.button === DialogButton.DELETE) {
        this.deleteResult(result);
      }
    });
  }

  private deleteResult(result: ListMySelfTestResultDto): void {
    this.apiService.deleteMySelfTestResult(result.resultId).subscribe({
      next: () => {
        this.selfTestResults = this.selfTestResults.filter(r => r.resultId !== result.resultId);
        this.toasterService.success('Self test result removed.');
      },
      error: (err) => {
        this.toasterService.error('Failed to remove the result. Try again.');
        console.error(err);
      }
    });
  }
}