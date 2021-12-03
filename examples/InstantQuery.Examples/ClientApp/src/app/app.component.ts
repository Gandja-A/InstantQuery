import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import * as moment from 'moment';
import { debounceTime, map, Observable } from 'rxjs';
import { OrderDetailsDto, OrdersClient } from './../api/apiClients';
import { KeyValuePairDto } from '../api/apiClients';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass'],
})
export class AppComponent {
  constructor(
    private formBuilder: FormBuilder,
    private ordersClient: OrdersClient
  ) {}

  displayedColumns: string[] = [
    'userFullName',
    'lotNumber',
    'createdAt',
    'quantity',
    'statusName',
  ];

  data: OrderDetailsDto[] = [];

  resultsLength = 0;

  isLoadingResults = true;

  filterBarForm: FormGroup = new FormGroup({});

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) sort: MatSort = new MatSort();

  statuses$!: Observable<any[]>;

  ngOnInit(): void {
    this.filterBarForm = this.formBuilder.group({
      startDate: [new Date(moment().subtract(1, 'month').format('yyyy-MM-DD'))],
      endDate: [new Date(moment().format('yyyy-MM-DD'))],
      selectedStatuses: [''],
      searchPattern: [''],
    });
    this.statuses$! = this.ordersClient.getOrderStatuses().pipe(
      map((items: KeyValuePairDto[]) => {
        return items.map((item) => {
          return { id: item.key, name: item.value };
        });
      })
    );

    this.filterBarForm.valueChanges
      .pipe(debounceTime(100))
      .subscribe(() => {
        this.paginator.pageIndex = 0;
        this.fetchData();
      });
  }

  ngAfterViewInit(): void {
    this.fetchData();
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
      this.fetchData();
    });
    this.paginator.page.subscribe(() => this.fetchData());
  }

  fetchData() {
    const {
      startDate,
      endDate,
      statusIds,
      searchPattern,
    }: {
      startDate: any;
      endDate: any;
      statusIds: number[];
      searchPattern: string;
    } = this.getQueryParams();
    this.isLoadingResults = true;
    this.ordersClient
      .getOrders(
        searchPattern,
        startDate,
        endDate,
        statusIds,
        this.paginator.pageIndex + 1,
        10,
        this.sort.active,
        this.sort.direction
      )
      .pipe(
        map((data) => {
          this.isLoadingResults = false;

          if (data === null) {
            return [];
          }
          this.resultsLength = data.totalCount ?? 0;
          return data.data;
        })
      )
      .subscribe((data) => {
        this.data = data as OrderDetailsDto[];
      });
  }

  private getQueryParams() {
    const statusIds: number[] =
      this.filterBarForm.controls['selectedStatuses'].value ?? null;
    const startDate = this.filterBarForm.controls['startDate'].value ?? null;
    const endDate = this.filterBarForm.controls['endDate'].value ?? null;
    const searchPattern =
      this.filterBarForm.controls['searchPattern'].value ?? null;
    return { startDate, endDate, statusIds, searchPattern };
  }
}
