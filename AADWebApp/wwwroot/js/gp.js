$(document).ready(function () {

    /**
     * When the modal show/hidden events are fired, add/remove the hidden input fields
     */
    $('#request-prescription-patient-modal')
        .on('show.bs.modal', function (event) {
                const patientModal = $(this);
                const triggerButton = $(event.relatedTarget);
                const parentRow = triggerButton.closest("tr");

                const medicationId = parentRow.data("medication-id");
                const prescriptionStartDate = parentRow.find('.js-start-date').val();
                const prescriptionEndDate = parentRow.find('.js-end-date').val();
                const medicationDosage = parentRow.find('.js-dosage').val();
                const medicationIssueFrequency = parentRow.find('.js-issue-frequency').val();

                const inputMedicationId = "<input type='hidden' name='MedicationId' value='" + medicationId + "'/>";
                const inputPrescriptionStartDate = "<input type='hidden' name='DateStart' value='" + prescriptionStartDate + "'/>";
                const inputPrescriptionEndDate = "<input type='hidden' name='DateEnd' value='" + prescriptionEndDate + "'/>";
                const inputMedicationDosage = "<input type='hidden' name='Dosage' value='" + medicationDosage + "'/>";
                const inputMedicationIssueFrequency = "<input type='hidden' name='IssueFrequency' value='" + medicationIssueFrequency + "'/>";
                const inputJoined = "<div id='hiddenInputs'>" + inputMedicationId + inputPrescriptionStartDate + inputPrescriptionEndDate + inputMedicationDosage + inputMedicationIssueFrequency + "</div>";

                const patientModalDialog = patientModal.find('form > .modal-dialog');
                $(inputJoined).insertBefore(patientModalDialog);
            }
        )
        .on('hidden.bs.modal', function () {
                const patientModal = $(this);
                const patientModalHiddenInputs = patientModal.find('form > #hiddenInputs');

                patientModalHiddenInputs.remove();
            }
        );
});