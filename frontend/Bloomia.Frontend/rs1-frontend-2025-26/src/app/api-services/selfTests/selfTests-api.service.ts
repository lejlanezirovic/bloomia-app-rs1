import { HttpClient } from "@angular/common/http";
import { inject,Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { GetSelfTestByIdQueryDto, ListAllSelfTestsQueryDto } from "./selfTests-api.models";
import { Observable } from "rxjs";

@Injectable({
    providedIn:'root'
})
export class SelfTestsApiService{
    //ovaj sloj komunicira sa apijem
    private baseUrl=`${environment.apiUrl}/api/SelfTests`;
    private http=inject(HttpClient);

    getAllSelfTests(){
        return this.http.get<ListAllSelfTestsQueryDto>(`${this.baseUrl}/get-all-self-tests`);
    }

    getSelfTestById(selfTestId:number) :Observable<GetSelfTestByIdQueryDto>{
        return this.http.get<GetSelfTestByIdQueryDto>(`${this.baseUrl}/get-self-test-by-id/${selfTestId}`);
    }
}