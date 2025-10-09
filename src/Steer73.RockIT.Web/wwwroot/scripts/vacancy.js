let jobFormResponse = null;
let diversityFormResponse = null;

const setupEvents = () => {
    const applyJobNext = document.querySelector('.js-job-1-next');
    applyJobNext.addEventListener('click', handleJob1Next);

    const job2Next = document.querySelector('.js-job-2-next');
    job2Next.addEventListener('click', handleDiversityStaticNext);
    const job2Skip = document.querySelector('.js-job-2-skip');
    job2Skip.addEventListener('click', handleDiversityStaticSkip);

    const fileUploadBoxes = document.querySelectorAll('.js-file-upload-box');
    fileUploadBoxes.forEach((fileUploadBox) => {
        fileUploadBox.addEventListener('click', triggerFileUpload);
    });

    const fileUploadInputs = document.querySelectorAll(
        '.form-file-upload input[type="file"]',
    );
    fileUploadInputs.forEach((fileUploadInput) => {
        fileUploadInput.addEventListener('change', handleFileUpload);
    });
};

const handleJob1Next = () => {
    if ($('.js-form-1').valid() && termsConditionsChecked()) {
        const jobStep1 = document.querySelector('.js-job-1');
        const jobStep2 = document.querySelector('.js-job-2');
        const jobStep3 = document.querySelector('.js-job-3');
        const jobStep4 = document.querySelector('.js-job-4');

        jobStep1.classList.add('d-none');
        if (showDiversityForm) {
            jobStep2.classList.remove('d-none');
        } else if (formDefinitionDetails) {
            loadSurveyForm();
            jobStep3.classList.remove('d-none');
        } else if (formDefinitionDiversity) {
            loadDiversitySurveyForm();
            jobStep4.classList.remove('d-none');
        } else {
            submitVacancy();
        }
        scrollToTop();
    }
};

const termsConditionsChecked = () => {
    const termsConditions = document.querySelector('.js-terms-conditions');
    if (!termsConditions.checked) {
        abp.notify.error('Please accept the terms and conditions to proceed.');
    }
    return termsConditions.checked;
};

const handleDiversityStaticNext = () => {
    const jobStep2 = document.querySelector('.js-job-2');
    const jobStep3 = document.querySelector('.js-job-3');
    const jobStep4 = document.querySelector('.js-job-4');

    jobStep2.classList.add('d-none');

    if (formDefinitionDetails) {
        loadSurveyForm();
        jobStep3.classList.remove('d-none');
    } else if (formDefinitionDiversity) {
        loadDiversitySurveyForm();
        jobStep4.classList.remove('d-none');
    } else {
        submitVacancy();
    }
    scrollToTop();
};

const handleDiversityStaticSkip = () => {
    const diversityForm = document.querySelector('.js-diversity-form');
    diversityForm.reset();
    handleDiversityStaticNext();
};

const loadSurveyForm = () => {
    let formDefinitionSurvey = new Survey.Model(formDefinitionDetails);
    const surveyFormContainer = document.querySelector('.js-job-3 .card-body');
    formDefinitionSurvey.showCompletedPage = false;
    addSkipOption(formDefinitionSurvey);
    formDefinitionSurvey.onComplete.add(surveyComplete);
    formDefinitionSurvey.applyTheme(surveyTheme);
    formDefinitionSurvey.render(surveyFormContainer);
};

const surveyComplete = (survey) => {
    jobFormResponse = survey.data;
    const jobStep3 = document.querySelector('.js-job-3');
    const jobStep4 = document.querySelector('.js-job-4');
    jobStep3.classList.add('d-none');
    if (formDefinitionDiversity) {
        loadDiversitySurveyForm();
        jobStep4.classList.remove('d-none');
    } else {
        submitVacancy();
    }
    scrollToTop();
};

const loadDiversitySurveyForm = () => {
    let diversityFormDefinition = new Survey.Model(formDefinitionDiversity);
    const diversityFormContainer = document.querySelector('.js-job-4 .card-body');
    diversityFormDefinition.showCompletedPage = false;
    addSkipOption(diversityFormDefinition);
    diversityFormDefinition.render(diversityFormContainer);
    diversityFormDefinition.applyTheme(surveyTheme);
    diversityFormDefinition.onComplete.add(diversitySurveyComplete);
};

const diversitySurveyComplete = (survey) => {
    diversityFormResponse = survey.data;
    const jobStep4 = document.querySelector('.js-job-4');
    jobStep4.classList.add('d-none');
    submitVacancy();
};

const submitVacancy = async () => {
    try {
        abp.ui.setBusy('.js-submit-message');
        toggleSubmitMessage(false);
        scrollToTop();
        const form = document.querySelector('.js-form-1');
        const formData = new FormData(form);
        if (jobFormResponse) {
            formData.append('JobFormResponse', JSON.stringify(jobFormResponse));
        }
        if (diversityFormResponse) {
            formData.append(
                'DiversityFormResponse',
                JSON.stringify(diversityFormResponse),
            );
        }

        const diversityForm = document.querySelector('.js-diversity-form');
        if (diversityForm) {
            const diversityFormData = new FormData(diversityForm);
            diversityFormData.forEach((value, key) => {
                console.log(key, value);
                formData.append(key, value);
            });
        }

        await abp.ajax({
            type: 'POST',
            url: '/ApplicantPortal/CreateJobApplication',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            abpHandleError: false // disable abp's default error handling from showing pop up.
        });

        toggleSubmitMessage(true);
        showSuccessMessage();
    }
    catch (error)
    {
        toggleSubmitMessage(true);
        showErrorMessage(error.message);

        if (error.validationErrors)
        {
            const errors = {};
            error.validationErrors.forEach(function (value, index, array) {
                value.members.forEach(function (member, index, array) {
                    const memberId = String(member).charAt(0).toUpperCase() + String(member).slice(1);
                    errors[memberId] = value.message;
                });
            });

            if (Object.keys(errors).length) {
                $('.js-form-1').validate().showErrors(errors);
                document.querySelector('.js-job-1').classList.remove('d-none');
                const errorMessage = document.querySelector('.js-error-message');
                errorMessage.classList.add('d-none');
            }
        }
    }
    finally {
        abp.ui.clearBusy();
    }
};

const showSuccessMessage = () => {
    const successMessage = document.querySelector('.js-success-message');
    successMessage.classList.remove('d-none');
};

const showErrorMessage = (message = "") => {
    const errorMessage = document.querySelector('.js-error-message');
    errorMessage.classList.remove('d-none');
    if (message) {
        errorMessage.firstChild.innerHTML = message;
    }
    else {
        errorMessage.firstChild.innerHTML = "Something went wrong";
    }
};

const toggleSubmitMessage = (flag) => {
    const submitMessage = document.querySelector('.js-submit-message');
    submitMessage.classList.toggle('d-none', flag);
};

const setupOtherInputToggle = () => {
    const radioInputs = document.querySelectorAll(
        '.js-diversity-form input[type="radio"]',
    );
    radioInputs.forEach((input) => {
        input.addEventListener('change', handleRadioChange);
    });
};

const handleRadioChange = (event) => {
    const selected = event.target.classList.contains('js-toggle-input');
    const jsOptionContainer = event.target.closest('.js-option-container');
    if (jsOptionContainer) {
        const otherInput = jsOptionContainer.querySelector('.js-other-input');
        if (otherInput) {
            otherInput.classList.toggle('d-none', !selected);
        }
    }
};

const scrollToTop = () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth',
    });
};

const addSkipOption = (surveyForm) => {
    surveyForm.addNavigationItem({
        title: 'Skip',
        action: () => {
            surveyForm.clear();
            surveyForm.completeLastPage();
        },
        css: 'nav-button',
        innerCss: 'js-skip-survey sd-btn nav-input',
    });
};

const triggerFileUpload = (event) => {
    const parentContainer = event.target?.closest('.form-file-upload');
    if (parentContainer) {
        const fileUploadInput = parentContainer.querySelector('input[type="file"]');
        if (fileUploadInput) {
            fileUploadInput.click();
        }
    }
};

const handleFileUpload = (event) => {
    const parentContainer = event.target?.closest('.form-file-upload');
    const spanTag = parentContainer.querySelector('.js-file-upload-box span');
    const fileUploadInput = parentContainer.querySelector('input[type="file"]');
    if (fileUploadInput && spanTag) {
        spanTag.textContent = fileUploadInput.files[0].name;
    }

    const iconTag = parentContainer.querySelector('.js-file-upload-box i');
    if (iconTag) {
        iconTag.classList.add('d-none');
    }
    const subtitleTag = parentContainer.querySelector('.subtitle');
    if (subtitleTag) {
        subtitleTag.classList.remove('d-none');
    }
};

// Call the setupEvents function when the DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    setupEvents();
    setupOtherInputToggle();
    setTimeout(() => {
        scrollToTop();
    }, 200);
});
