// Chuyển tab Login / Register
function showForm(formType) {
  document.querySelectorAll('.form').forEach(form => form.classList.remove('active'));
  document.querySelectorAll('.tab').forEach(tab => tab.classList.remove('active'));

  document.getElementById(`${formType}-form`).classList.add('active');
  document.querySelector(`.tab[onclick="showForm('${formType}')"]`).classList.add('active');
}

// Xử lý form Đăng nhập
document.getElementById('loginForm').addEventListener('submit', async (e) => {
  e.preventDefault();

  const username = document.getElementById('login-username').value.trim();
  const password = document.getElementById('login-password').value.trim();
  const errorEl = document.getElementById('login-error');

  if (!username || !password) {
    errorEl.textContent = 'Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu';
    return;
  }

  try {
    const response = await fetch('http://localhost:5000/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password }),
    });

    const data = await response.json();

    console.log('Response từ API đăng nhập:', data); // Debug: xem toàn bộ response

    if (!response.ok) {
      throw new Error(data.message || `Đăng nhập thất bại (mã lỗi: ${response.status})`);
    }

    // Lấy token trực tiếp từ data.token (dựa trên response bạn cung cấp)
    const token = data.token;
    if (!token) {
      throw new Error('Không tìm thấy token trong phản hồi từ server');
    }

    localStorage.setItem('token', token);
    alert('Đăng nhập thành công!');
    window.location.href = 'dashboard.html'; // Chuyển sang trang dashboard
  } catch (err) {
    console.error('Lỗi đăng nhập:', err);
    errorEl.textContent = err.message;
  }
});

// Xử lý đăng nhập Google (callback từ Google)
function handleGoogleLogin(response) {
  fetch('http://localhost:5000/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ googleId: response.credential }),
  })
  .then(res => res.json())
  .then(data => {
    console.log('Response Google login:', data); // Debug

    if (data.success) {
      const token = data.token;  // Lấy trực tiếp data.token
      if (!token) {
        throw new Error('Không tìm thấy token từ Google login');
      }

      localStorage.setItem('token', token);
      alert('Đăng nhập bằng Google thành công!');
      window.location.href = 'dashboard.html';
    } else {
      document.getElementById('login-error').textContent = data.message || 'Lỗi đăng nhập Google';
    }
  })
  .catch(err => {
    console.error('Lỗi Google:', err);
    document.getElementById('login-error').textContent = 'Lỗi kết nối hoặc server';
  });
}

// Xử lý form Đăng ký
document.getElementById('registerForm').addEventListener('submit', async (e) => {
  e.preventDefault();

  const username = document.getElementById('reg-username').value.trim();
  const password = document.getElementById('reg-password').value.trim();
  const email = document.getElementById('reg-email').value.trim();
  const fullName = document.getElementById('reg-fullname').value.trim();
  const phoneNumber = document.getElementById('reg-phone').value.trim();
  const errorEl = document.getElementById('register-error');

  if (!username || !password || !email) {
    errorEl.textContent = 'Vui lòng điền đầy đủ các trường bắt buộc (Username, Password, Email)';
    return;
  }

  try {
    const response = await fetch('http://localhost:5000/api/auth/register', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password, email, fullName, phoneNumber }),
    });

    const data = await response.json();

    console.log('Response từ API đăng ký:', data); // Debug

    if (!response.ok) {
      throw new Error(data.message || `Đăng ký thất bại (mã lỗi: ${response.status})`);
    }

    alert('Đăng ký thành công! Hãy đăng nhập.');
    showForm('login');
  } catch (err) {
    console.error('Lỗi đăng ký:', err);
    errorEl.textContent = err.message;
  }
});