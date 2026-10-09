export interface AdminAnalyticsRequest {
  dateFrom: string;
  dateTo: string;
}

export interface AdminBusinessAnalyticsDto {
  appointmentsCount: number;
  completedSessionsCount: number;
  averageRating: number;
}

export interface AdminUserAnalyticsDto {
  clientsCount: number;
  therapistsCount: number;
  activeClientsCount: number;
}

export interface AdminSystemPerformanceDto {
  totalRequests: number;
  averageResponseTimeMs: number;
  serverErrorsCount: number;
  errorRate: number;
}

export interface AdminAnalyticsDto {
  business: AdminBusinessAnalyticsDto;
  users: AdminUserAnalyticsDto;
  systemPerformance: AdminSystemPerformanceDto;
}