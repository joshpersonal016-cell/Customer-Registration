import Popup from 'devextreme-react/popup'
import Form, { Item, GroupItem, Label, RequiredRule } from 'devextreme-react/form'
import Button from 'devextreme-react/button'

import type { CreateCustomerDto } from '../types/customer'

type Props = {
  visible: boolean
  onClose: () => void
  onSubmit?: () => void
  formData: CreateCustomerDto
  setFormData: React.Dispatch<React.SetStateAction<CreateCustomerDto>>
  readOnly?: boolean
}

export default function CustomerModal({
  visible,
  onClose,
  onSubmit,
  formData,
  setFormData,
  readOnly,
}: Props) {
  const handleChange = (e: any) => {
    if (readOnly) return

    setFormData((prev) => ({
      ...prev,
      [e.dataField]: e.value,
    }))
  }

  return (
    <Popup
      visible={visible}
      onHiding={onClose}
      showCloseButton={true}
      hideOnOutsideClick={true}
      dragEnabled={true}
      showTitle={true}
      title={readOnly ? 'Customer Details' : 'Add Customer'}
      width={526}
      height={578}
    >
      {/* 🌟 MODAL WRAPPER STYLE */}
      <div
        style={{
          padding: 20,
          background: '#fff',
          borderRadius: 10,
        }}
      >
        <Form
          formData={formData}
          onFieldDataChanged={handleChange}
          colCount={2}
          labelLocation="top"
          readOnly={readOnly}
        >
          <GroupItem colSpan={2} caption="Customer Information">
            <Item dataField="firstName">
              <Label text="First Name" />
              <RequiredRule message="Required" />
            </Item>

            <Item dataField="lastName">
              <Label text="Last Name" />
              <RequiredRule message="Required" />
            </Item>

            <Item dataField="email">
              <Label text="Email" />
            </Item>

            <Item dataField="phoneNumber">
              <Label text="Phone Number" />
            </Item>
          </GroupItem>
        </Form>

        {/* 🌟 BUTTON AREA */}
        {!readOnly && (
          <div
            style={{
              marginTop: 25,
              display: 'flex',
              justifyContent: 'flex-end',
              gap: 10,
            }}
          >
            <Button
              text="Cancel"
              type="normal"
              stylingMode="outlined"
              onClick={onClose}
            />

            <Button
              text="Save Customer"
              type="default"
              stylingMode="contained"
              onClick={onSubmit}
            />
          </div>
        )}
      </div>
    </Popup>
  )
}