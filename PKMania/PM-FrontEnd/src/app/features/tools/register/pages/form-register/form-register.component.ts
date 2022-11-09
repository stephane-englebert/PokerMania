import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { RegisterService } from '../../../login/services/register.service';

@Component({
  selector: 'app-form-register',
  templateUrl: './form-register.component.html',
  styleUrls: ['./form-register.component.css']
})
export class FormRegisterComponent implements OnInit {

  formGroupAddMember!: FormGroup;

  constructor(
    private _formBuilder: FormBuilder,
    private _registerService: RegisterService,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {
  }

  ngOnInit(): void {
    this.formGroupAddMember = this._formBuilder.group({
      Pseudo: [null, [Validators.required, Validators.maxLength(30)]],
      Email: [null, [Validators.required, Validators.maxLength(150)]],
      Password: [null, [Validators.required, Validators.maxLength(30)]],
      VerifPassword: [null, [Validators.required, Validators.maxLength(30)]]
    })
  }

  submit() {
    if (this.formGroupAddMember.invalid) {
      this.translate.get("REGISTER.GENERAL.INVALID").subscribe(toastMsg => {
        this.toastr.error(toastMsg);
      });
      return;
    }
    this._registerService.addMember(this.formGroupAddMember.value).subscribe({
      next: (res) => {
        this.translate.get("REGISTER.GENERAL.SUCCESS").subscribe(toastMsg => {
          this.toastr.success(toastMsg);
        });
      },
      error: (err) => {
        this.translate.get("REGISTER.GENERAL.ERROR").subscribe(toastMsg => {
          this.toastr.error(toastMsg);
        });
      }
    });
  }
}
