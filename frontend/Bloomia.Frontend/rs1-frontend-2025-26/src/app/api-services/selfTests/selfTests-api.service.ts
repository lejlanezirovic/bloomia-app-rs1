import { HttpClient } from "@angular/common/http";
import { inject,Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { GetSelfTestByIdQueryDto, ListAllSelfTestsQueryDto,SubmitSelfTestCommand
         ,SubmitSelfTestCommandDto,SelfTestAnswersCommandDto } from "./selfTests-api.models";
import { Observable } from "rxjs";
import { buildHttpParams } from "../../core/models/build-http-params";
import {
  ListMySelfTestResultsQuery, ListMySelfTestResultsResponse, GetMySelfTestResultByIdDto,
  UpdateSelfTestResultNoteCommand, UpdateSelfTestResultNoteCommandDto
} from "./selfTests-api.models";

@Injectable({
    providedIn:'root'
})
export class SelfTestsApiService{
    
    private baseUrl=`${environment.apiUrl}/api/SelfTests`;
    private http=inject(HttpClient);

    getAllSelfTests(){
        return this.http.get<ListAllSelfTestsQueryDto>(`${this.baseUrl}/get-all-self-tests`);
    }

    getSelfTestById(selfTestId:number) :Observable<GetSelfTestByIdQueryDto>{
        return this.http.get<GetSelfTestByIdQueryDto>(`${this.baseUrl}/get-self-test-by-id/${selfTestId}`);
    }
    submitSelfTest(selfTest:SubmitSelfTestCommand):Observable<SubmitSelfTestCommandDto>{
        return this.http.post<SubmitSelfTestCommandDto>
                                (`${this.baseUrl}/create-client-self-test`,selfTest);
    }
        getMySelfTestResults(query: ListMySelfTestResultsQuery): Observable<ListMySelfTestResultsResponse> {
        const params = buildHttpParams(query as any);
        return this.http.get<ListMySelfTestResultsResponse>(`${this.baseUrl}/my-results`, { params });
    }

    getMySelfTestResultById(resultId: number): Observable<GetMySelfTestResultByIdDto> {
        return this.http.get<GetMySelfTestResultByIdDto>(`${this.baseUrl}/my-results/${resultId}`);
    }

    updateMySelfTestResultNote(resultId: number, command: UpdateSelfTestResultNoteCommand): Observable<UpdateSelfTestResultNoteCommandDto> {
        return this.http.put<UpdateSelfTestResultNoteCommandDto>(`${this.baseUrl}/my-results/${resultId}/note`, command);
    }

    deleteMySelfTestResult(resultId: number): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/my-results/${resultId}`, { responseType: 'text' as 'json' });
    }
}