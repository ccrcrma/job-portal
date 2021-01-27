$(function() {
    $("form[name='password-reset']").validate({
      rules: {
        email: {
            required: true,
            email: true
        }
      },

      // Make sure the form is submitted to the destination defined
      // in the "action" attribute of the form when valid
      submitHandler: function(form) {
        console.log("form submitted");
        form.submit();
      }
    });
  });