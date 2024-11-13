// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function setSelectedRole() {
    var selectedRole = document.getElementById("roleSelect").value;

    document.getElementById("selectedRole").value = selectedRole;
}

function getUserDetails(userId) {
    fetch(`/Team/GetUserDetails/${userId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
            if (data.error) {
                document.getElementById('userDetails').innerHTML = `<p>${data.error}</p>`;
                return;
            }

            document.getElementById('userDetails').innerHTML = `
                <h3>${data.name} ${data.surname.toUpperCase()}</h3>
                <p class="d-inline"><strong>Position:</strong> ${data.position}</p>
                <p class="d-inline"><strong>Team:</strong> ${data.team}</p>
                <p class="d-inline"><strong>Email:</strong> ${data.email}</p>

                <div class="accordion mt-5" id="accordionExample">
                    ${data.tasks.map((task, index) => `
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="heading${index}">
                                <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${index}" aria-expanded="false" aria-controls="collapse${index}">
                                    ${task.taskHeader}  ${task.taskStatus === 2 ? ' - Finished' : task.taskStatus === 0 ? ' - Not Started' :' - In Progerss'}
                                </button>
                            </h2>
                            <div id="collapse${index}" class="accordion-collapse collapse" aria-labelledby="heading${index}" data-bs-parent="#accordionExample">
                                <div class="accordion-body">
                                    <h3>Description:</h3> 
                                    <div class="fs-3"> ${task.taskDescription} </div>
                                    <strong>Added Date:</strong> ${new Date(task.taskAddedDate).toLocaleDateString()}
                                    <strong>Status:</strong><div class="status-circle ${task.taskStatus === 0 ? 'bg-danger' : task.taskStatus === 1 ? 'bg-success' : 'bg-secondary'}"></div>
                                </div>
                            </div>
                        </div>
                    `).join('')}
                </div>
            `;
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}




function SelectSurnameAndId()
{
    const select = document.getElementById("UserData");
    const selectedOption = select.options[select.selectedIndex];
    const userIdField = document.getElementById("UserId");
    const userSurnameField = document.getElementById("UserSurname");

    userIdField.value = selectedOption.getAttribute("data-id");
    userSurnameField.value = selectedOption.getAttribute("data-surname");

}

//function updateClock() {
//    var now = new Date();
//    var hour = now.getHours().toString().padStart(2, '0');
//    var minutes = now.getMinutes().toString().padStart(2, '0');
//    var seconds = now.getSeconds().toString().padStart(2, '0');

//    var timeString = hour + ":" + minutes + ":" + seconds;
//    document.getElementById('clock').textContent = timeString;
//}

//setInterval(updateClock, 1000);
//updateClock();

const email = "test@test.com";
const pass = "Akin12345!";

function autologin() {
    document.querySelector("#email-login").value = email;
    document.querySelector("#pwd-login").value = pass;
    document.querySelector("#btn-login").click();
}

if (document.querySelector("#email-login") && document.querySelector("#pwd-login")) {
    autologin();
};