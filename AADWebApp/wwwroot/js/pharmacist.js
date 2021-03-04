$(document).ready(function () {

    /**
     * When the modal show/hidden events are fired, add/remove the hidden input fields
     */
    $('#request-bloodwork-modal')
        .on('show.bs.modal', function (event) {
                const requestBloodworkModal = $(this);
                const triggerButton = $(event.relatedTarget);
                const parentRow = triggerButton.closest("tr");                

                const prescriptionId = parentRow.data("prescription-id");

                const inputPrescriptionId = "<input type='hidden' name='PrescriptionId' value='" + prescriptionId + "'/>";
                const inputJoined = "<div id='hiddenInputs'>" + inputPrescriptionId + "</div>";

                const patientModalDialog = requestBloodworkModal.find('form > .modal-dialog');
                $(inputJoined).insertBefore(patientModalDialog);
            }
        )
        .on('hidden.bs.modal', function () {
                const requestBloodworkModal = $(this);
                const requestBloodworkModalHiddenInputs = requestBloodworkModal.find('form > #hiddenInputs');

                requestBloodworkModalHiddenInputs.remove();
            }
        );
});