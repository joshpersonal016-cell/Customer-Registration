import { useEffect, useState } from 'react'
import CustomerGrid from '../components/CustomerGrid'
import CustomerModal from '../components/CustomerModal'
import {
  createCustomerAsync,
  getCustomersAsync,
  getCustomerByIdAsync,
} from '../services/customerService'
import type { Customer } from '../types/customer'

export default function CustomerPage() {
  const [customers, setCustomers] = useState<Customer[]>([])
  const [loading, setLoading] = useState(false)

  const [open, setOpen] = useState(false)
  const [mode, setMode] = useState<'add' | 'view'>('add')

  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
  })

  useEffect(() => {
    loadCustomers()
  }, [])

  const loadCustomers = async () => {
    setLoading(true)
    const data = await getCustomersAsync()
    setCustomers(data)
    setLoading(false)
  }

  // ➕ ADD CUSTOMER
  const openAdd = () => {
    setMode('add')

    setFormData({
      firstName: '',
      lastName: '',
      email: '',
      phoneNumber: '',
    })

    setOpen(true)
  }

  // 👁 VIEW CUSTOMER (CALL API)
  const openView = async (id: string) => {
    setMode('view')
    setOpen(true)

    const data = await getCustomerByIdAsync(id)

    setFormData({
      firstName: data.firstName,
      lastName: data.lastName,
      email: data.email,
      phoneNumber: data.phoneNumber,
    })
  }

  // 💾 CREATE CUSTOMER
  const createCustomer = async () => {
    await createCustomerAsync(formData)

    setOpen(false)
    loadCustomers()
  }

  return (
    <div style={{ padding: 20 }}>
      <h2>Customers</h2>

      <button onClick={openAdd}>+ Add Customer</button>

      <CustomerGrid
        data={customers}
        loading={loading}
        onView={openView}
      />

      <CustomerModal
        visible={open}
        onClose={() => setOpen(false)}
        onSubmit={mode === 'add' ? createCustomer : undefined}
        formData={formData}
        setFormData={setFormData}
        readOnly={mode === 'view'}
      />
    </div>
  )
}