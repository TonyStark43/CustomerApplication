function loadDistrictsByState(stateId) {
    if (stateId) {
        $.ajax({
            url: '/Customer/Index?handler=DistrictsByState',
            type: 'GET',
            data: { stateId: stateId },
            success: function (data) {
                $('#districtDropdown').empty().append('<option value="">Select District</option>');
                $.each(data, function (index, item) {
                    $('#districtDropdown').append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            },
            error: function () {
                alert('Failed to load districts.');
            }
        });
    } else {
        $('#districtDropdown').empty().append('<option value="">Select District</option>');
    }
}

function loadDistrictsByStateEdit(stateId) {
    if (stateId) {
        $.ajax({
            url: '/Customer/Index?handler=DistrictsByState',
            type: 'GET',
            data: { stateId: stateId },
            success: function (data) {
                $('#editCustomerDistrict').empty().append('<option value="">Select District</option>');
                $.each(data, function (index, item) {
                    $('#editCustomerDistrict').append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            },
            error: function () {
                alert('Failed to load districts.');
            }
        });
    } else {
        $('#editCustomerDistrict').empty().append('<option value="">Select District</option>');
    }
}



//function loadCustomerData2(id) {
//    $.ajax({
//        url: '/Customer/Index?handler=CustomerData',
//        type: 'GET',
//        data: { id: id },
//        success: function (data) {
//            if (data) {
//                $('#editCustomerId').val(data.id);
//                $('#editCustomerName').val(data.name);
//                $('#editCustomerGender').val(data.genderId);
//                $('#editCustomerState').val(data.stateId);
//                // Call a method to load districts based on the stateId
//                loadDistrictsByState(data.stateId, 'editCustomerDistrict', data.districtId);
//                $('#editCustomerModal').modal('show');
//            } else {
//                alert('Customer not found');
//            }
//        },
//        error: function () {
//            alert('Failed to fetch customer data');
//        }
//    });
//}



//function loadDistrictsByState3(selectedStateId) {
//    // Clear the district dropdown
//    $('#editCustomerDistrict').empty();
//    $('#editCustomerDistrict').append($('<option>').text('Select District').attr('value', ''));

//    // Fetch districts based on the selected state
//    $.ajax({
//        url: '/your-endpoint-to-fetch-districts', // Replace with the actual endpoint
//        type: 'GET',
//        data: { stateId: selectedStateId },
//        success: function (districts) {
//            $.each(districts, function (i, district) {
//                $('#editCustomerDistrict').append($('<option>').text(district.name).attr('value', district.id));
//            });
//        },
//        error: function () {
//            console.error('Failed to load districts.');
//        }
//    });
//}



// Function to load dropdown data for the update
function loadDropdownsWithValues(genderId, stateId, districtId, genders, states, districts) {
    // Load Genders
    
    let genderDropdown = $('#editCustomerGender');
    genderDropdown.empty();
    genderDropdown.append($('<option>').text('Select Gender').attr('value', ''));

    $.each(genders, function (i, gender) {
        let option = $('<option>').text(gender.genderName).attr('value', gender.id);
        if (gender.id === genderId) {
            option.prop('selected', true);
        }
        genderDropdown.append(option);
    });

    // Load States
    let stateDropdown = $('#editCustomerState');
    stateDropdown.empty();
    stateDropdown.append($('<option>').text('Select State').attr('value', ''));
    $.each(states, function (i, state) {
        let option = $('<option>').text(state.name).attr('value', state.id);
        if (state.id === stateId) {
            option.prop('selected', true);
        }
        stateDropdown.append(option);
    });

    // Load Districts
    let districtDropdown = $('#editCustomerDistrict');
    districtDropdown.empty();
    districtDropdown.append($('<option>').text('Select District').attr('value', ''));
    $.each(districts, function (i, district) {
        let option = $('<option>').text(district.name).attr('value', district.id);
        if (district.id === districtId) {
            option.prop('selected', true);
        }
        districtDropdown.append(option);
    });
}


// Function to load dropdown data for the create
function loadDropdowns() {
    $.ajax({
        url: '/Customer/Index?handler=DropdownData',  // Ensure this URL matches your server route
        type: 'GET',
        success: function (data) {
            populateDropdown('#stateDropdown', data.states);
            populateDropdown('#genderDropdown', data.genders);
        },
        error: function () {
            alert('Failed to load dropdown data.');
        }
    });
}

// Helper function to populate a dropdown
function populateDropdown(selector, items) {
    $(selector).empty().append('<option value="">Select</option>');
    $.each(items, function (index, item) {
        $(selector).append('<option value="' + item.id + '">' + item.name + '</option>');
    });
}



//search functionality
function enableApplyButtonBySearch() {
    const searchName = $('#searchName').val().trim();
    const searchButton = $('#searchButton');

    if (searchName.length > 0) {
        searchButton.prop('disabled', false);
    } else {
        searchButton.prop('disabled', true);
    }
}
function enableApplyButtonBydd() {
    const searchButton = $('#searchButton');
    searchButton.prop('disabled', false);
}

function filteredCustomerList() {
    var searchName = '';
    if ($('#searchName').length) {
        searchName = $('#searchName').val().trim();
    }
    const sortBy = $('#sortBy').val(); // Get selected sort option
    const pageSize = parseInt($('#pageSize').val());
    
    loadCustomerList(searchName, 1, pageSize, sortBy);
}




//Intial Customer/Index page load call
$(function () {
    if (window.location.pathname.toLowerCase().includes('/customer/index')
        || window.location.pathname.toLowerCase() == '/customer'
        || window.location.pathname.toLowerCase() == '/customer/') {
        loadCustomerList('', 1, 10, 'id_asc');
    }
});

//Load customers list
function loadCustomerList(searchName, pageNum, pageSize, sortBy) {
    const searchButton = $('#searchButton');

    $('#searchButton').prop('disabled', true);
    

   
    $.ajax({
        url: '/Customer/Index?handler=LoadCustomerList',
        type: 'GET',
        data: {
            filterName: searchName ,                    // Filter by name
            pageNum: pageNum,                           // Pagination: Page number
            pageSize: pageSize,                         // Pagination: Page size
            sortBy: sortBy                              // Sort by column
        },
        success: function (data) {
            $('#customerListContainer').html(data);

            paginationHandleWrapper(pageNum, pageSize)
            
        },
        error: function () {
            alert('Failed to load customer list.');
        }
    });
}

function paginationHandleWrapper(pageNum, pageSize) {
    

    $.ajax({
        url: '/Customer/Index?handler=TotalCustomerCount',
        type: 'GET',
        
        success: function (data) {
            
            setPaginationControls(data.count, pageNum, pageSize)

        },
        error: function () {
            alert('Failed to load customer count.');
        }
    });
}

//Create customer
function createCustomer() {
    // Collect form data
    var form = $('#createCustomerForm');
    form.validate();
    if (form.valid()) {
        var formData = form.serialize();
        $.ajax({
            url: '/Customer/Create?handler=CreateCustomer', // Get the URL from the form's data-url attribute
            type: 'POST',
            data: formData,
            success: function (response) {
                // Handle successful response
                if (response.success) {
                    alert('Customer created successfully!');
                    $('#createCustomerModal').modal('hide'); // Hide modal
                    
                    loadCustomerList('', 1, 10, 'id_asc');
                } else {
                    alert('Error creating customer: ' + response.message);
                }
            },
            error: function () {
                alert('An error occurred while creating the customer.');
            }
        });
    }
}

//Delete customer
function confirmDelete(customerId) {
   
    if (confirm("Are you sure you want to delete this Customer?")) {
        
        $.ajax({
            url: '/Customer/Create?handler=DeleteCustomer', 
            type: 'POST',
            data: { customerId: customerId },
            success: function (response) {
                
                alert("Customer deleted successfully!");
                
                loadCustomerList('', 1, 10, 'id_asc');
            },
            error: function () {
                // Handle errors
                alert("Error deleting Customer.");
            }
        });
    } else {
        
        return false;
    }
}

//UpdateCustomer
function updateCustomer() {
    var form = $('#editCustomerForm');
    form.validate();
    if (form.valid()) {
        var formData = form.serialize();


        $.ajax({
            url: '/Customer/Create?handler=UpdateCustomer', // Endpoint for updating customer
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    // Close the modal after a successful update
                    alert("Customer updated successfully!");
                    $('#editCustomerModal').modal('hide');

                    loadCustomerList('', 1, 10, 'id_asc');
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Error updating customer.");
            }
        });
    }
}

//Customer details by id for view
function loadCustomerDetails(customerId) {
    $.ajax({
        url: '/Customer/Index?handler=GetCustomerDetails', // Replace with your actual endpoint
        type: 'GET',
        data: { id: customerId },
        success: function (data) {
            $('#customerDetailsContent').html(data);
        },
        error: function () {
            alert('Failed to load customer details.');
        }
    });
}

//customer details for edit
function loadCustomerData(id) {
    $.ajax({
        url: '/Customer/Index?handler=CustomerData',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            if (data) {
                // Prefill the form fields with customer data
                $('#editCustomerId').val(data.customer.id);
                $('#editCustomerName').val(data.customer.name);

                // Load dropdowns with prefilled values
                loadDropdownsWithValues(data.customer.genderId, data.customer.stateId, data.customer.districtId, data.genders, data.states, data.districts);

                // Open the modal
                $('#editCustomerModal').modal('show');
            } else {
                alert('Customer not found');
            }
        },
        error: function (xhr) {
            var errorMessage = xhr.responseJSON ? xhr.responseJSON.error : 'Failed to fetch customer data';
            alert(errorMessage);
        }
    });
}






// reset page filter, sorts
function resetPageControles() {
    $('#pageSize').val('10'); 

    $('#sortBy').val('id_asc');

    $('#searchName').val('');

    $('#searchButton').prop('disabled', true);
}



document.addEventListener('DOMContentLoaded', function () {
    // Attach click event listeners to the buttons
    document.getElementById('previousPage').addEventListener('click', function () {
        handlePageChange('previous');
    });

    document.getElementById('nextPage').addEventListener('click', function () {
        handlePageChange('next');
    });
});

function handlePrevPageClick() {
    handlePageChange('previous');
}
function handleNextPageClick() {
    handlePageChange('next');
}

function handlePageChange(direction) {
    // Get the current page number
    var pageNumberElement = document.getElementById('pageNumber');

    // Ensure the text content is a string and trim any extra spaces
    var textContent = pageNumberElement.textContent.trim();

    // Convert to a number and default to 1 if conversion fails
    var currentPage = Number(textContent) || 1;

    // Determine the new page number
    var newPage;
    if (direction === 'previous') {
        newPage = currentPage > 1 ? currentPage - 1 : 1; // Ensure the page number doesn't go below 1
    } else if (direction === 'next') {
        newPage = currentPage + 1; // Update based on your pagination logic
    }

    var searchName = '';
    if ($('#searchName').length) {
        searchName = $('#searchName').val().trim();
    }
    const sortBy = $('#sortBy').val(); // Get selected sort option
    const pageSize = parseInt($('#pageSize').val());

    loadCustomerList(searchName, newPage, pageSize, sortBy);

}



function setPaginationControls(totalCount, currentPage, pageSize) {
    // Calculate total pages, using Math.ceil to ensure you round up to the next whole page
    var totalPage = Math.ceil(totalCount / pageSize);

    // Disable "Next" button if currentPage is the last page
    var nextPageButton = document.getElementById('nextPage');
    if (currentPage >= totalPage) {
        nextPageButton.disabled = true;
    } else {
        nextPageButton.disabled = false;
    }

    // Disable "Previous" button if on the first page
    var previousPageButton = document.getElementById('previousPage');
    if (currentPage <= 1) {
        previousPageButton.disabled = true;
    } else {
        previousPageButton.disabled = false;
    }

    // Update the page number displayed
    var pageNumberElement = document.getElementById('pageNumber');
    pageNumberElement.textContent = currentPage;
}






$(function () {
    // Get the anti-forgery token from the meta tag
    var csrfToken = $('meta[name="csrf-token"]').attr('content');

    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': csrfToken
        }
    });
});


function resetCustomerForm() {
    // Clear text inputs
    document.getElementById('customerNameCreate').value = '';

    var genderDropdown = document.getElementById('genderDropdown');
    genderDropdown.selectedIndex = 0; // Set to default or initial value


    var stateDropdown = document.getElementById('stateDropdown');
    stateDropdown.selectedIndex = 0; // Set to default or initial value

    var districtDropdown = document.getElementById('districtDropdown');
    districtDropdown.selectedIndex = 0; // Set to default or initial value

}