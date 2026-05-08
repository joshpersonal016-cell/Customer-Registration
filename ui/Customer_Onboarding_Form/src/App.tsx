import { Routes, Route, Navigate } from 'react-router-dom'
import CustomerPage from './pages/CustomerPage'

export default function App() {
  return (
    <Routes>
      {/* Default route */}
      <Route path="/" element={<Navigate to="/customers" />} />

      {/* Customer page */}
      <Route path="/customers" element={<CustomerPage />} />
    </Routes>
  )
}