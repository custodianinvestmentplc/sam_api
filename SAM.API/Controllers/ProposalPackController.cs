using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using SAM.NUGET;
using SAM.NUGET.Domain.Options;
using SAM.NUGET.Models;
using SAM.NUGET.Payloads;
using SAM.NUGET.Services;
using SAM.NUGET.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CPC.API.Controllers
{
    [Route("api/proposal-pack")]
    [ApiController]
    public class ProposalPackController : ControllerBase
    {
        private readonly ICPCHubServices _cpcServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProposalPackController(ICPCHubServices cpcHubServices, IWebHostEnvironment webHostEnvironment)
        {
            _cpcServices = cpcHubServices;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public IActionResult CreateProposalPack([FromBody] CreateProposalPackOptions payload)
        {
            try
            {
                var refNbr = _cpcServices.CreateProposalPack(payload);

                return StatusCode(201, refNbr);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CreateProposalPackError"
                });
            }
        }


        [HttpPost]
        [Route("adduser")]
        public IActionResult AddNewUser([FromBody] UserProfileOptions payload)
        {
            try
            {
                var refNbr = _cpcServices.AddNewUser(payload);

                return StatusCode(201, refNbr);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "AddNewUserError"
                });
            }
        }

        [HttpPost]
        [Route("add-role")]
        public IActionResult AddRoleSettings([FromBody] AddNewRoleOptions payload)
        {
            try
            {
                var refNbr = _cpcServices.AddRoleSettings(payload);

                return StatusCode(201, refNbr);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "AddRoleSettingsError"
                });
            }
        }

        [HttpPost]
        [Route("add-template")]
        public IActionResult AddTemplateSettings([FromBody] AddNewTemplateOptions payload)
        {
            try
            {
                var refNbr = _cpcServices.AddTemplateSettings(payload);

                return StatusCode(201, refNbr);

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Add{payload.Template}SettingsError"
                });
            }
        }

        [HttpPost]
        [Route("edit-user")]
        public IActionResult UpdateUser([FromBody] UserProfileOptions payload)
        {
            try
            {
                _cpcServices.UpdateUserProfile(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "UpdatedUserProfileError"
                });
            }
        }

        [HttpPost]
        [Route("edit-role")]
        public IActionResult EditRoleSettings([FromBody] AddNewRoleOptions payload)
        {
            try
            {
                _cpcServices.EditRoleSettings(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "UpdatedRoleSettingsError"
                });
            }
        }

        [HttpPost]
        [Route("edit-template")]
        public IActionResult EditTemplateSettings([FromBody] AddNewTemplateOptions payload)
        {
            try
            {
                _cpcServices.EditTemplateSettings(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Updated{payload.Template}SettingsError"
                });
            }
        }

        [HttpGet]
        [Route("reference-nbr")]
        public IActionResult GetProposalPack([FromQuery] string refnbr)
        {
            try
            {
                var proposalPack = _cpcServices.GetProposalPackByRefNumber(refnbr);

                return StatusCode(200, proposalPack);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(404, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("user-profile")]
        public IActionResult GetUserProfile([FromQuery] string refnbr)
        {
            try
            {
                var userProfile = _cpcServices.GetUserProfileByRefNumber(refnbr);

                return StatusCode(200, userProfile);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(404, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetUserProfileError"
                });
            }
        }

        [HttpGet]
        [Route("get-role")]
        public IActionResult GetRole([FromQuery] string refnbr)
        {
            try
            {
                var role = _cpcServices.GetRoleSettings(refnbr);

                return StatusCode(200, role);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(404, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetRoleSettingsError"
                });
            }
        }

        [HttpGet]
        [Route("get-template")]
        public IActionResult GetTemplate([FromQuery] string refnbr, string templateType, string useremail)
        {
            try
            {
                var payload = new TemplateIdSettingsOptions()
                {
                    ReferenceNbr = refnbr,
                    TemplateType = templateType,
                    AddedBy = useremail
                };

                var template = _cpcServices.GetTemplateSettings(payload);

                return StatusCode(200, template);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(404, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Get{templateType}SettingsError"
                });
            }
        }

        [HttpGet]
        [Route("users-profile")]
        public IActionResult GetUsersProFile()
        {
            try
            {
                var userProfiles = _cpcServices.GetUserProfiles();

                return StatusCode(200, userProfiles);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetUserProfilesError"
                });
            }
        }

        [HttpGet]
        [Route("roles-settings")]
        public IActionResult GetRolesSettings()
        {
            try
            {
                var roles = _cpcServices.GetCpcRoles();

                return StatusCode(200, roles);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CpcRolesRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("template-settings")]
        public IActionResult GetTemplatesSettings([FromQuery] string templateType, string useremail)
        {
            try
            {
                var payload = new TemplateIdSettingsOptions()
                {
                    TemplateType = templateType,
                    AddedBy = useremail
                };

                var templates = _cpcServices.GetCpcTemplates(payload);

                return StatusCode(200, templates);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Cpc{templateType}sRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("drafts")]
        public IActionResult GetDraftProposalPack()
        {
            try
            {
                var proposalPack = _cpcServices.GetAllDraftProposalPacks();

                return StatusCode(200, proposalPack);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetDraftProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("submits")]
        public IActionResult GetSubmittedProposalPack()
        {
            try
            {
                var proposalPack = _cpcServices.GetAllSubmittedProposalPacks();

                return StatusCode(200, proposalPack);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetSubmittedProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("inbounds")]
        public IActionResult GetInboundProposalPacks([FromQuery] string useremail)
        {
            try
            {
                var proposalPacks = _cpcServices.GetAllInboundProposalPacks(useremail);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetInboundProposalPacksError"
                });
            }
        }

        [HttpGet]
        [Route("wip")]
        public IActionResult GetWIPProposalPacks([FromQuery] string useremail)
        {
            try
            {
                var proposalPacks = _cpcServices.GetAllWIPProposalPacks(useremail);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetWIPProposalPacksError"
                });
            }
        }

        [HttpGet]
        [Route("accepted")]
        public IActionResult GetAcceptedProposalPacks([FromQuery] string useremail)
        {
            try
            {
                var proposalPacks = _cpcServices.GetAllAcceptedProposalPacks(useremail);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetAcceptedProposalPacksError"
                });
            }
        }

        [HttpGet]
        [Route("approved")]
        public IActionResult GetApprovedProposalPacks([FromQuery] string useremail)
        {
            try
            {
                var proposalPacks = _cpcServices.GetAllApprovedProposalPacks(useremail);

                return StatusCode(200, proposalPacks);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetApprovedProposalPacksError"
                });
            }
        }

        [HttpGet]
        [Route("content-types")]
        public IActionResult FetchProposalPackContentType()
        {
            try
            {
                var model = _cpcServices.FetchProposalPackContentTypes();

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetDraftProposalPackError"
                });
            }
        }

        [HttpGet]
        [Route("contents")]
        public IActionResult GetProposalPackContents([FromQuery] string refNbr)
        {
            try
            {
                var model = _cpcServices.GetProposalPackContents(refNbr);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("contents/delete")]
        public IActionResult DeleteProposalPackContent([FromBody] DeleteProposalPackContentRequest payload)
        {
            try
            {
                var result = _cpcServices.DeleteProposalPackContent(payload.ProposalPackReferenceNbr, payload.ProposalPackContentRowId);

                if (result)
                {
                    return StatusCode(201, "Success");
                }

                return StatusCode(404);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("files/delete")]
        public IActionResult DeleteProposalPackFile([FromBody] DeleteProposalPackFileRequest payload)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                var result = _cpcServices.DeleteProposalPackFile(payload);

                if (result)
                {
                    var uploads = webRootPath + WebConstants.ImagePath;

                    var fileName = uploads + payload.proposalPackDocName;

                    ControllerHelper.DeleteFile(fileName);

                    return StatusCode(201, "Success");
                }

                return StatusCode(404);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("delete-user")]
        public IActionResult DeleteUserProfile([FromBody] DeleteUserProfile payload)
        {
            try
            {
                var result = _cpcServices.DeleteUserProfile(payload.UserReferenceNbr, payload.RowId);

                if (result)
                {
                    return StatusCode(201, "Success");
                }

                return StatusCode(404);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteUserProfileError"
                });
            }
        }

        [HttpPost]
        [Route("delete-role")]
        public IActionResult DeleteRoleSettings([FromBody] DeleteRoleSettings payload)
        {
            try
            {
                var result = _cpcServices.DeleteRoleSettings(payload.ReferenceNbr);

                if (result)
                {
                    return StatusCode(201, "Success");
                }

                return StatusCode(404);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteRoleSettingsError"
                });
            }
        }

        [HttpPost]
        [Route("delete-template")]
        public IActionResult DeleteTemplateSettings([FromBody] TemplateIdSettings payload)
        {
            try
            {
                var result = _cpcServices.DeleteTemplateSettings(payload);

                if (result)
                {
                    return StatusCode(201, "Success");
                }

                return StatusCode(404);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = $"Delete{payload.TemplateType}SettingsError"
                });
            }
        }

        [HttpPost]
        [Route("contents")]
        public IActionResult AddProposalPackContentRecord([FromBody] AddProposalPackContentRecordOption payload)
        {
            try
            {
                var model = _cpcServices.AddProposalPackContentRecord(payload);

                if (model.NewRecordId > 0)
                {
                    var newrecord = _cpcServices.GetProposalPackContentRecord(payload.ReferenceNumber, model.NewRecordId);
                    return StatusCode(201, newrecord);
                }

                return StatusCode(404);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpGet]
        [Route("contents/record")]
        public IActionResult GetProposalPackContentRecord([FromQuery] string refNbr, [FromQuery] string rowId)
        {
            try
            {
                var newrecord = _cpcServices.GetProposalPackContentRecord(refNbr, Convert.ToDecimal(rowId));
                return StatusCode(200, newrecord);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/general-info")]
        public IActionResult SaveTraditionalBusinessGeneral([FromBody] ProposalFormTradGeneralOption payload)
        {
            try
            {
                _cpcServices.SaveProposalFormTradGeneral(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/general-info")]
        public IActionResult FindTraditionalBusinessStep1([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep1(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/tax-info")]
        public IActionResult SaveTraditionalBusinessTaxDetails([FromBody] ProposalFormTradTaxDetails payload)
        {
            try
            {
                _cpcServices.SaveProposalFormTradTaxDetails(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "DeleteProposalPackContentError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/tax-info")]
        public IActionResult FindTraditionalBusinessStep2([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep2(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/identity-info")]
        public IActionResult SaveTraditionalBusinessIdentificationDetails([FromBody] ProposalFormTradIdentification payload)
        {
            try
            {
                _cpcServices.SaveProposalFormTradIdentificationDetails(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/identity-info")]
        public IActionResult FindTraditionalBusinessStep3([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep3(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/bank-info")]
        public IActionResult SaveTraditionalBusinessBankInfo([FromBody] ProposedFormTradBankInfo payload)
        {
            try
            {
                _cpcServices.SaveProposalFormTradBankInfo(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/bank-info")]
        public IActionResult FindTraditionalBusinessStep4([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep4(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/mortgage-info")]
        public IActionResult SaveTraditionalBusinessMortgageInfo([FromBody] ProposalFormTradMortgageInfo payload)
        {
            try
            {
                _cpcServices.SaveProposalFormTradMortgageInfo(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/mortgage-info")]
        public IActionResult FindTraditionalBusinessStep5([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep5(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/children-education")]
        public IActionResult SaveTraditionalBusinessChildrenEducation([FromBody] ProposedFormTradChildrenEducation payload)
        {
            try
            {
                _cpcServices.SaveProposalFormTradChildrenEducation(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/children-education")]
        public IActionResult FindTraditionalBusinessStep6([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep6(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/digital-plan")]
        public IActionResult SaveTraditionalBusinessDigitalPlan([FromBody] List<NewDigitalPlanNomineeForm> payload)
        {
            try
            {
                var ops = new DigitalPlanOperationDetails();

                if (payload != null && payload.Count > 0)
                {
                    var itm = payload[0];

                    ops.IsApplicable = itm.IsApplicable;
                    ops.ReferenceNbr = itm.ReferenceNbr;
                    ops.ContentTypeCode = itm.ContentTypeCode;
                    ops.UserEmail = itm.UserEmail;
                }
                else
                {
                    throw new Exception("Bad Request: Payload is Null or Empty");
                }

                _cpcServices.SaveProposalFormTradDigitalPlan(payload, ops);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/digital-plan")]
        public IActionResult FindTraditionalBusinessStep7([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep7(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/beneficiaries")]
        public IActionResult AddBeneficiaryToProposalForm([FromBody] AddBeneficiaryOption payload)
        {
            try
            { 
                var newId = _cpcServices.AddBeneficiaryToProposalFormTraditional(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/delete-beneficiary")]
        public IActionResult DeleteBeneficiaryFromProposalForm([FromBody] DeleteBeneficiaryForm payload)
        {
            try
            {
                _cpcServices.DeleteBeneficiaryFromProposalFormTraditional(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/save-draft-beneficiary")]
        public IActionResult SaveDraftBeneficiary([FromBody] SaveDraftBeneficiaryForm payload)
        {
            try
            {
                _cpcServices.SaveDraftBeneficiaryAsActive(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/beneficiaries")]
        public IActionResult FindTraditionalBusinessStep8([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep8(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/sum-assured")]
        public IActionResult SaveSumAssured([FromBody] NewDataCaptureSumAssured payload)
        {
            try
            {
                _cpcServices.SaveProposalFormTradSumAssured(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/sum-assured")]
        public IActionResult FindTraditionalBusinessStep9([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                log.Info($"{RefNbr} - {ContentTypeCode}");

                var model = _cpcServices.FindDataCaptureFormTraditionalStep9(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/other-insurer")]
        public IActionResult SaveOtherInsurerDetails([FromBody] AddOtherInsurerOption payload)
        {
            try
            {
                string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                log.Info($"{DateTime.Now} - {payloadstring}"); 

                _cpcServices.SaveProposalFormTradOtherInsurer(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/other-insurer")]
        public IActionResult FindTraditionalBusinessStep10([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep10(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/medical-history")]
        public IActionResult SaveMedicalHistory([FromBody] AddMedicalHistoryOption payload)
        {
            try
            {
                string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                log.Info($"{DateTime.Now} - {payloadstring}"); 

                _cpcServices.SaveProposalFormTradMedicalHistory(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/medical-history")]
        public IActionResult FindTraditionalBusinessStep11([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep11(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/misc-details")]
        public IActionResult SaveMiscellaneousDetails([FromBody] AddMiscellaneousOption payload)
        {
            try
            {
                _cpcServices.SaveProposalFormTradMisc(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/misc-details")]
        public IActionResult FindTraditionalBusinessStep12([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep12(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/traditional/other-medical-info")]
        public IActionResult SaveOtherMedicalInfo([FromBody] AddOtherMedicalInfo payload)
        {
            try
            {
                string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                log.Info($"{DateTime.Now} - {payloadstring}"); 

                _cpcServices.SaveProposalFormTradOtherMedicalInfo(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/other-medical-info")]
        public IActionResult FindTraditionalBusinessStep13([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
        {
            try
            {
                var model = _cpcServices.FindDataCaptureFormTraditionalStep13(RefNbr, ContentTypeCode);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("data-capture/traditional/supporting-docs")]
        public IActionResult SaveSupportingDocs([FromBody] SupportingDocPayload payload)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                var uploads = webRootPath + WebConstants.ImagePath;

                if (payload.Data.Count > 0 && payload.Data != null)
                {
                    if (payload.Data[0] != null)
                    {
                        var fileExtention = Path.GetExtension(payload.Data[0].FileName);

                        string docFileName = ((payload.ReferenceNbr + "_" + payload.DocType).Replace("/", "")) + fileExtention;

                        if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

                        var filename = uploads + docFileName;

                        ControllerHelper.DeleteFile(filename);

                        ControllerHelper.SaveFileToDirectory(payload.Data[0], filename);

                        var supportingFile = new SupportingDocFile()
                        {
                            FileName = docFileName,
                            Name = payload.Data[0].Name,
                            ContentType = payload.Data[0].ContentType,
                            Size = payload.Data[0].Length,
                            ReferenceNbr = payload.ReferenceNbr,
                            DocType = payload.DocType,
                            LastUpdatedUser = payload.UserEmail,
                        };

                        _cpcServices.SaveSupportingDoc(supportingFile);

                        return StatusCode(201);
                    }

                    else
                    {
                        return StatusCode(500, new
                        {
                            ErrorDescription = "File is Empty",
                            ExceptionType = "SaveDataCaptureFormError"
                        });
                    }
                }

                else
                {
                    return StatusCode(500, new
                    {
                        ErrorDescription = "No File Uploaded",
                        ExceptionType = "SaveDataCaptureFormError"
                    });
                }

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SaveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/supporting-doc")]
        public IActionResult GetFile([FromQuery] string RefNbr, [FromQuery] string DocType)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                //var uploads = webRootPath + WebConstants.ImagePath;
                var uploads = @"https://localhost:7178/cpc_files/";

                var existingFile = _cpcServices.GetFile(RefNbr, DocType);


                if (existingFile != null)
                {
                    existingFile.FileUrl = uploads + existingFile.FileName;

                    return StatusCode(200, existingFile);
                }
                else
                {
                    return StatusCode(500, new
                    {
                        ErrorDescription = "No record found",
                        ExceptionType = "RetrieveDataCaptureFormError"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RetrieveDataCaptureFormError"
                });
            }
        }

        [HttpGet]
        [Route("data-capture/traditional/supporting-docs")]
        public IActionResult GetSupportingDocs([FromQuery] string RefNbr)
        {
            try
            {
                var files = _cpcServices.GetSupportingDocs(RefNbr);

                return StatusCode(200, files);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetSupportingDocsError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/submit-content")]
        public IActionResult SubmitProposalPackContent([FromBody] SubmitProposalPackContentForm payload)
        {
            try
            {
                _cpcServices.SubmitProposalPackContent(payload.ReferenceNbr, payload.ContentTypeCode);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SubmitProposalPackContentError"
                });
            }
        }

        [HttpPost]
        [Route("data-capture/submit-proposal-pack")]
        public IActionResult SubmitProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                log.Info($"{DateTime.Now} - SubmitProposalPack Action Method - {payloadstring}");

                _cpcServices.SubmitProposalPack(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SubmitProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("inbounds")]
        public IActionResult PickInboundProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                _cpcServices.PickInboundProposalPack(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "PickInboundProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("accept-pack")]
        public IActionResult AcceptInboundProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                _cpcServices.AcceptInboundProposalPack(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "AcceptInboundProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("reject-pack")]
        public IActionResult RejectInboundProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                _cpcServices.RejectInboundProposalPack(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "RejectInboundProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("push-pack")]
        public IActionResult PushInboundProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                _cpcServices.PushInboundProposalPack(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "PushInboundProposalPackError"
                });
            }
        }

        [HttpPost]
        [Route("approve-pack")]
        public IActionResult ApproveProposalPack([FromBody] SubmitProposalPackForm payload)
        {
            try
            {
                _cpcServices.ApproveProposalPack(payload);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "ApproveProposalPackError"
                });
            }
        }
    }
}







//return StatusCode(500, new
//{
//    ErrorDescription = "No File Uploaded",
//    ExceptionType = "SaveDataCaptureFormError"
//});







//[HttpPost]
//[Consumes("multipart/form-data")]
//[Route("data-capture/traditional/supporting-docs")]
//public IActionResult SaveSupportingDocs([FromQuery] string RefNbr, [FromQuery] string DocType)
//{
//    try
//    {
//        var data = Request.Form.Files;

//        string webRootPath = _webHostEnvironment.WebRootPath;
//        
//        var uploads = webRootPath + WebConstants.ImagePath;

//        if (data.Count > 0 && data != null)
//        {
//            if (data[0] != null)
//            {
//                var fileExtention = Path.GetExtension(data[0].FileName);

//                string docFileName = ((RefNbr + "_" + DocType).Replace("/", "")) + fileExtention;

//                if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

//                var filename = uploads + docFileName;

//                ControllerHelper.DeleteFile(filename);

//                ControllerHelper.SaveFileToDirectory(data[0], filename);

//                var supportingFile = new SupportingDocFile()
//                {
//                    FileName = docFileName,
//                    Name = data[0].Name,
//                    ContentType = data[0].ContentType,
//                    Size = data[0].Length,
//                    ReferenceNbr = RefNbr,
//                    DocType = DocType,
//                    LastUpdatedUser = userEmail,
//                };

//                _cpcServices.SaveSupportingDoc(supportingFile);
//            }

//        }

//        else
//        {
//            var existingFile = _cpcServices.GetFile(RefNbr, DocType);

//            existingFile.DocType = DocType;
//            existingFile.LastUpdatedUser = userEmail;

//            _cpcServices.SaveSupportingDoc(existingFile);
//        }

//        return StatusCode(201, new
//        {
//            OperationOutcome = "Success"
//        });

//    }
//    catch (Exception ex)
//    {
//        log.Error(DateTime.Now.ToString(), ex);

//        return StatusCode(500, new
//        {
//            ErrorDescription = ex.Message,
//            ExceptionType = "SaveDataCaptureFormError"
//        });
//    }
//}







//[HttpPost]
//[Consumes("multipart/form-data")]
//[Route("data-capture/traditional/supporting-docs")]
//public IActionResult SaveSupportingDocs([FromQuery] string RefNbr, [FromQuery] string ContentTypeCode)
//{
//    var data = Request.Form.Files;

//    string webRootPath = _webHostEnvironment.WebRootPath;

//    if (data.Count > 0 && data != null)
//    {
//        try
//        {
//            
//            var uploads = webRootPath + WebConstants.ImagePath;

//            var fileId = data.First(i => i.Name == WebConstants.IdDocTypeName);
//            var filePassport = data.First(i => i.Name == WebConstants.PassportDocTypeName);
//            var fileUtility = data.First(i => i.Name == WebConstants.UtilityDocTypeName);

//            if (fileId != null)
//            {
//                var fileExtention = Path.GetExtension(fileId.FileName);
//                string docFileName = ((RefNbr + ContentTypeCode + WebConstants.IdDocTypeName).Replace("/", "")) + fileExtention;

//                if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

//                var idDoc = ControllerHelper.HandleFormFile(fileId, uploads, RefNbr, ContentTypeCode, WebConstants.IdDocTypeName, docFileName);
//                _cpcServices.SaveSupportingIdDocs(idDoc, userEmail);
//            }

//            if (filePassport != null)
//            {
//                var fileExtention = Path.GetExtension(filePassport.FileName);
//                string docFileName = ((RefNbr + ContentTypeCode + WebConstants.PassportDocTypeName).Replace("/", "")) + fileExtention;

//                if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

//                var passportDoc = ControllerHelper.HandleFormFile(filePassport, uploads, RefNbr, ContentTypeCode, WebConstants.PassportDocTypeName, docFileName);
//                _cpcServices.SaveSupportingIdDocs(passportDoc, userEmail);
//            }

//            if (fileUtility != null)
//            {
//                var fileExtention = Path.GetExtension(fileUtility.FileName);
//                string docFileName = ((RefNbr + ContentTypeCode + WebConstants.UtilityDocTypeName).Replace("/", "")) + fileExtention;

//                if (docFileName.Length > 50) docFileName = docFileName.Remove(0, docFileName.Length - 50);

//                var utilityDoc = ControllerHelper.HandleFormFile(fileUtility, uploads, RefNbr, ContentTypeCode, WebConstants.UtilityDocTypeName, docFileName);
//                _cpcServices.SaveSupportingIdDocs(utilityDoc, userEmail);
//            }


//            return StatusCode(201, new
//            {
//                OperationOutcome = "Success"
//            });
//        }
//        catch (Exception ex)
//        {
//            log.Error(DateTime.Now.ToString(), ex);

//            return StatusCode(500, new
//            {
//                ErrorDescription = ex.Message,
//                ExceptionType = "SaveDataCaptureFormError"
//            });
//        }
//    }
//    else
//    {
//        return StatusCode(500, new
//        {
//            ErrorDescription = "No File Uploaded",
//            ExceptionType = "SaveDataCaptureFormError"
//        });
//    }
//}











//[HttpGet]
//[Route("data-capture/traditional/supporting-doc")]
//public IActionResult GetFile([FromQuery] string RefNbr, string ContentTypeCode)
//{
//    string webRootPath = _webHostEnvironment.WebRootPath;

//    try
//    {
//        //var uploads = webRootPath + WebConstants.ImagePath;

//        var uploads = @"https://localhost:7178/cpc_files/";

//        var existingIdFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, WebConstants.IdDocTypeName);
//        var existingPassportFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, WebConstants.PassportDocTypeName);
//        var existingUtilityFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, WebConstants.UtilityDocTypeName);


//        if (existingIdFile != null || existingPassportFile != null || existingUtilityFile != null)
//        {
//            existingIdFile.FileUrl = uploads + existingIdFile.FileName;
//            //existingIdFile.FileUrl = @"https://images.freeimages.com/images/large-previews/411/platform-1154314.jpg";
//            existingPassportFile.FileUrl = uploads + existingPassportFile.FileName;
//            //existingPassportFile.FileUrl = @"https://images.freeimages.com/images/large-previews/411/platform-1154314.jpg";
//            existingUtilityFile.FileUrl = uploads + existingUtilityFile.FileName;
//            //existingUtilityFile.FileUrl = @"https://images.freeimages.com/images/large-previews/411/platform-1154314.jpg";

//            var model = new Step14DataCaptureFormTraditionalDto
//            {
//                ContentTypeCode = ContentTypeCode,
//                ReferenceNbr = RefNbr,
//                IdFIle = existingIdFile,
//                PassportFile = existingPassportFile,
//                UtilityBillFile = existingUtilityFile
//            };
//            //"/Users/apple/Desktop/CPC(3)/CPCHub/app-login/wwwroot/cpc_files/comedy-logo-illustration-theme-performances-stand-up-etc-178212650.jpg"
//            return StatusCode(200, new
//            {
//                FoundRecord = model != null,
//                Data = model
//            });
//        }
//        else
//        {
//            return StatusCode(500, new
//            {
//                ErrorDescription = "No record found",
//                ExceptionType = "RetrieveDataCaptureFormError"
//            });
//        }
//    }
//    catch (Exception ex)
//    {
//        log.Error(DateTime.Now.ToString(), ex);

//        return StatusCode(500, new
//        {
//            ErrorDescription = ex.Message,
//            ExceptionType = "RetrieveDataCaptureFormError"
//        });
//    }
//}






//var existingIdFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, "id_file");
//var existingPassportFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, "passport_file");
//var existingUtilityFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(RefNbr, ContentTypeCode, "utility_file");


//if (existingIdFile != null || existingPassportFile != null || existingUtilityFile != null)
//{
//    var existingFiles = new List<SupportingDocFile>()
//                        {
//                            existingIdFile,
//                            existingPassportFile,
//                            existingUtilityFile
//                        };

//    foreach (var item in existingFiles)
//    {
//        var oldFile = uploads + item.FileName;

//        if (System.IO.File.Exists(oldFile))
//        {
//            System.IO.File.Delete(oldFile);
//        }
//    }
//}



//model.IdFIle = await ControllerHelper.CreateFormFileAsync(existingIdFile, uploads);
//model.PassportFile = await ControllerHelper.CreateFormFileAsync(existingPassportFile, uploads);
//model.UtilityBillFile = await ControllerHelper.CreateFormFileAsync(existingUtilityFile, uploads);


//ExistingIdFIle = existingIdFile,
//ExistingPassportFile = existingPassportFile,
//ExistingUtilityBillFile = existingUtilityFile



//                    catch (Exception e)
//{
//    Response.Clear();
//    Response.StatusCode = 204;
//    Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
//    Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
//}





//[HttpPost]
//[Route("data-capture/traditional/supporting-docs")]
//public IActionResult SaveSupportingDocs([FromBody] NewSupportingDocsForm payload)
//{
//    string webRootPath = _webHostEnvironment.WebRootPath;

//    try
//    {
//        string payloadstring = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
//        log.Info($"{DateTime.Now} - {payloadstring}");

//        
//        var uploads = webRootPath + WebConstants.ImagePath;

//        //var model = _cpcServices.FindDataCaptureFormTraditionalStep14(payload.ReferenceNbr, payload.ContentTypeCode);
//        var existingIdFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(payload.ReferenceNbr, payload.ContentTypeCode, "id_card");
//        var existingPassportFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(payload.ReferenceNbr, payload.ContentTypeCode, "passport");
//        var existingUtilityFile = _cpcServices.FindDataCaptureFormTraditionalStep14IdFile(payload.ReferenceNbr, payload.ContentTypeCode, "utility_bill");

//        var existingFiles = new List<SupportingDocFile>()
//                    {
//                        existingIdFile,
//                        existingPassportFile,
//                        existingUtilityFile
//                    };


//        foreach (var item in existingFiles)
//        {
//            var oldFile = uploads + $@"\{item.FileUrl}";

//            if (System.IO.File.Exists(oldFile))
//            {
//                System.IO.File.Delete(oldFile);
//            }
//        }

//        var idDoc = ControllerHelper.HandleFile(payload.IdFIle[0], uploads, payload.ReferenceNbr, payload.ContentTypeCode, "id_card");
//        var passportDoc = ControllerHelper.HandleFile(payload.PassportFile[0], uploads, payload.ReferenceNbr, payload.ContentTypeCode, "passport");
//        var utilityDoc = ControllerHelper.HandleFile(payload.UtilityBillFile[0], uploads, payload.ReferenceNbr, payload.ContentTypeCode, "utility_bill");

//        _cpcServices.SaveSupportingIdDocs(idDoc, userEmail);
//        _cpcServices.SaveSupportingIdDocs(passportDoc, userEmail);
//        _cpcServices.SaveSupportingIdDocs(utilityDoc, userEmail);

//        return StatusCode(201, new
//        {
//            OperationOutcome = "Success"
//        });
//    }
//    catch (Exception ex)
//    {
//        log.Error(DateTime.Now.ToString(), ex);

//        return StatusCode(500, new
//        {
//            ErrorDescription = ex.Message,
//            ExceptionType = "SaveDataCaptureFormError"
//        });
//    }

//}