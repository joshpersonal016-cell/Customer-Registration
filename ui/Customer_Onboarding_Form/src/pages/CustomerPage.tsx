import { useEffect, useState } from 'react'
import CustomerGrid from '../components/CustomerGrid'
import CustomerModal from '../components/CustomerModal'
import {
  createCustomerAsync,
  getCustomersAsync,
  getCustomerByIdAsync,
} from '../services/customerService'
import type { Customer, CreateCustomerDto } from '../types/customer'

type CustomerForm = {
  firstName: string
  lastName: string
  email: string
  phoneNumber: string

  signatureUrl: string | null   // display only
  signatureBlob: Blob | null    // upload only
}

export default function CustomerPage() {
  const [customers, setCustomers] = useState<Customer[]>([])
  const [loading, setLoading] = useState(false)

  const [open, setOpen] = useState(false)
  const [mode, setMode] = useState<'add' | 'view'>('add')

  const [formData, setFormData] = useState<CustomerForm>({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    signatureUrl: null,
    signatureBlob: null,
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

  // ➕ ADD
  const openAdd = () => {
    setMode('add')

    setFormData({
      firstName: '',
      lastName: '',
      email: '',
      phoneNumber: '',
      signatureUrl: null,
      signatureBlob: null,
    })

    setOpen(true)
  }

  // 👁 VIEW
  const openView = async (id: string) => {
    setMode('view')
    setOpen(true)

    const data = await getCustomerByIdAsync(id)

    setFormData({
      firstName: data.firstName,
      lastName: data.lastName,
      email: data.email,
      phoneNumber: data.phoneNumber,
      signatureUrl: data.signatureUrl || null,
      signatureBlob: null,
    })
  }

  // 💾 CREATE (IMPORTANT FIX HERE)
  const createCustomer = async () => {
    const dto: CreateCustomerDto = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      email: formData.email,
      phoneNumber: formData.phoneNumber,
      signature: formData.signatureBlob,
    }

    await createCustomerAsync(dto)

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
