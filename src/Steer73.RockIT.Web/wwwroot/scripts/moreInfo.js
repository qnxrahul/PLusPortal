const setupEvents = () => {
  const submitButton = document.querySelector('.js-submit');
  submitButton.addEventListener('click', handleSubmit);
};

const handleSubmit = (event) => {
  event.preventDefault();
  if ($('.js-moreInfo-form').valid() && termsConditionsChecked()) {
    const form = document.querySelector('.js-moreInfo-form');
    form.submit();
  }
};

const termsConditionsChecked = () => {
  const termsConditions = document.querySelector('.js-terms-conditions');
  if (!termsConditions.checked) {
    abp.notify.error('Please accept the terms and conditions to proceed.');
  }
  return termsConditions.checked;
};

// Call method on DOM load
document.addEventListener('DOMContentLoaded', () => {
  setupEvents();
});
